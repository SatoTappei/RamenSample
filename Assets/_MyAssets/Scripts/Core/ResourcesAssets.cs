using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace PSB.Ramen
{
    /// <summary>
    /// このシーンで使用する動的に生成されるアセットを非同期でロードし、保持する
    /// ロードしたアセットは列挙型のキーを指定して取得できる
    /// ビルド後にAddressableを用いたロードが上手くいかないので、Resourcesを用いた同期ロードを行う
    /// </summary>
    public class ResourcesAssets : MonoBehaviour
    {
        [System.Serializable]
        public class AssetData
        {
            [field: SerializeField] public string Name { get; private set; }
            [field: SerializeField] public AssetKey Key { get; private set; }
        }

        // 単体で動作させるため、Start時に自動でロードするフラグ
        // 投げっぱなしなので終了を待たない
        readonly bool LoadOnStart = false;

        [Header("ロードするアセットのデータ")]
        [SerializeField] AssetData[] _assetData;
        [Header("進捗を表示するUI")]
        [SerializeField] LoadProgress _progress;

        Dictionary<AssetKey, GameObject> _assets = new();

        /// <summary>
        /// アセットのロードが済み、参照や生成が可能な状態になったフラグ
        /// </summary>
        public bool IsLoaded { get; private set; }
        
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
            if (!IsLoaded)
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
            if (IsLoaded)
            {
                Debug.LogWarning("既にアセットをロード済み");
                return false;
            }
            IsLoaded = true;

            // 進捗の表示
            _progress?.Play();

            foreach(AssetData data in _assetData)
            {
                // Resourcesを用いた非同期ロード
                ResourceRequest request = Resources.LoadAsync<GameObject>(data.Name);
                await UniTask.WaitUntil(() => request.isDone, cancellationToken: token);

                GameObject asset = request.asset as GameObject;
                if (asset == null)
                {
                    throw new System.NullReferenceException("アセットのロードに失敗: " + data.Key);
                }

                _assets.Add(data.Key, asset);
            }

            await UniTask.WaitForSeconds(1.0f, cancellationToken: token);

            // 進捗の表示を止める
            _progress?.Stop();

            return true;
        }
    }
}
