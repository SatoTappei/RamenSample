using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UniRx;
using UniRx.Triggers;
using UnityEngine.Events;

namespace PSB.Ramen
{
    /// <summary>
    /// このシーンで使用する動的に生成されるアセットを一括でロードし、保持する
    /// ロードしたアセットは列挙型のキーを指定して取得できる
    /// </summary>
    public class DynamicAssets : MonoBehaviour
    {
        [System.Serializable]
        public class AssetData
        {
            [field: SerializeField] public string Name { get; private set; }
            [field: SerializeField] public AssetKey Key { get; private set; }
            [field: SerializeField] public string Address { get; private set; }
        }

        // 単体で動作させるため、Start時に自動でロードするフラグ
        // 投げっぱなしなので終了を待たない
        readonly bool LoadOnStart = false;

        [Header("ロードするアセットのデータ")]
        [SerializeField] AssetData[] _assetData;
        [Header("進捗を表示するUI")]
        [SerializeField] LoadProgress _progress;

        Dictionary<AssetKey, GameObject> _assets = new();
        bool isLoaded;

        void Start()
        {
            if (LoadOnStart)
            { 
                LoadAllAsync(this.GetCancellationTokenOnDestroy()).Forget();
            }
        }

        /// <summary>
        /// 読み込んだアセットを取得する
        /// </summary>
        public GameObject Get(AssetKey key)
        {
            if (!isLoaded)
            {
                throw new System.InvalidOperationException("アセットのロードがされていない状態: " + key);
            }

            if (_assets.TryGetValue(key, out GameObject value)) return value;
            throw new KeyNotFoundException("キーに対応するアセットが見つからなかった: " + key);
        }

        /// <summary>
        /// 読み込んだアセットを生成して返す
        /// </summary>
        public GameObject Instantiate(AssetKey key, Vector3 pos = default, Quaternion rot = default, Transform parent = null)
        {
            rot = rot == default ? Quaternion.identity : rot;
            return Instantiate(Get(key), pos, rot, parent);
        }

        /// <summary>
        /// 全てのアセットを非同期でロードする
        /// </summary>
        public async UniTask<bool> LoadAllAsync(CancellationToken token)
        {
            if (isLoaded)
            {
                Debug.LogWarning("既にアセットをロード済み");
                return false;
            }
            isLoaded = true;

            // 進捗の表示
            _progress?.Play();

            List<AsyncOperationHandle<GameObject>> handles = new();
            List<UniTask<GameObject>> tasks = new();

            // このオブジェクトが破棄されるタイミングでロードしたアセットを解放する
            this.OnDestroyAsObservable().Subscribe(_ => 
            {
                handles.ForEach(h => Addressables.Release(h));
                handles.Clear();
            });

            foreach (AssetData data in _assetData)
            {
                AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(data.Address);
                handles.Add(handle);

                // 各々のUniTaskにCancellationTokenを渡すと、WhenAllで待ち受けた時に
                // どれか1つがキャンセルされた場合に全てキャンセルされる
                tasks.Add(handle.ToUniTask(cancellationToken: token));
            }

            // WhenAllの省略形
            GameObject[] assets = await tasks;

            // 全てのアセットのロードが成功したかチェック
            foreach(UniTask<GameObject> t in tasks)
            {
                if (t.Status != UniTaskStatus.Succeeded)
                {
                    Debug.LogError($"アセットのロードに成功しなかった: {t} {t.Status}");
                    return false;
                }
            }

            // 配列の先頭から辞書型に詰めていく
            // インスペクターで設定した順でロードするので配列の中身を順に対応させてけば良い
            for (int i = 0; i < _assetData.Length; i++)
            {
                _assets.Add(_assetData[i].Key, assets[i]);
            }

            // 進捗の表示を止める
            _progress?.Stop();

            return true;
        }
    }
}