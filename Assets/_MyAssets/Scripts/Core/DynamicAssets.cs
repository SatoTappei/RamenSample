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
    /// ���̃V�[���Ŏg�p���铮�I�ɐ��������A�Z�b�g���ꊇ�Ń��[�h���A�ێ�����
    /// ���[�h�����A�Z�b�g�͗񋓌^�̃L�[���w�肵�Ď擾�ł���
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

        // �P�̂œ��삳���邽�߁AStart���Ɏ����Ń��[�h����t���O
        // �������ςȂ��Ȃ̂ŏI����҂��Ȃ�
        readonly bool LoadOnStart = false;

        [Header("���[�h����A�Z�b�g�̃f�[�^")]
        [SerializeField] AssetData[] _assetData;
        [Header("�i����\������UI")]
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
        /// �ǂݍ��񂾃A�Z�b�g���擾����
        /// </summary>
        public GameObject Get(AssetKey key)
        {
            if (!isLoaded)
            {
                throw new System.InvalidOperationException("�A�Z�b�g�̃��[�h������Ă��Ȃ����: " + key);
            }

            if (_assets.TryGetValue(key, out GameObject value)) return value;
            throw new KeyNotFoundException("�L�[�ɑΉ�����A�Z�b�g��������Ȃ�����: " + key);
        }

        /// <summary>
        /// �ǂݍ��񂾃A�Z�b�g�𐶐����ĕԂ�
        /// </summary>
        public GameObject Instantiate(AssetKey key, Vector3 pos = default, Quaternion rot = default, Transform parent = null)
        {
            rot = rot == default ? Quaternion.identity : rot;
            return Instantiate(Get(key), pos, rot, parent);
        }

        /// <summary>
        /// �S�ẴA�Z�b�g��񓯊��Ń��[�h����
        /// </summary>
        public async UniTask<bool> LoadAllAsync(CancellationToken token)
        {
            if (isLoaded)
            {
                Debug.LogWarning("���ɃA�Z�b�g�����[�h�ς�");
                return false;
            }
            isLoaded = true;

            // �i���̕\��
            _progress?.Play();

            List<AsyncOperationHandle<GameObject>> handles = new();
            List<UniTask<GameObject>> tasks = new();

            // ���̃I�u�W�F�N�g���j�������^�C�~���O�Ń��[�h�����A�Z�b�g���������
            this.OnDestroyAsObservable().Subscribe(_ => 
            {
                handles.ForEach(h => Addressables.Release(h));
                handles.Clear();
            });

            foreach (AssetData data in _assetData)
            {
                AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(data.Address);
                handles.Add(handle);

                // �e�X��UniTask��CancellationToken��n���ƁAWhenAll�ő҂��󂯂�����
                // �ǂꂩ1���L�����Z�����ꂽ�ꍇ�ɑS�ăL�����Z�������
                tasks.Add(handle.ToUniTask(cancellationToken: token));
            }

            // WhenAll�̏ȗ��`
            GameObject[] assets = await tasks;

            // �S�ẴA�Z�b�g�̃��[�h�������������`�F�b�N
            foreach(UniTask<GameObject> t in tasks)
            {
                if (t.Status != UniTaskStatus.Succeeded)
                {
                    Debug.LogError($"�A�Z�b�g�̃��[�h�ɐ������Ȃ�����: {t} {t.Status}");
                    return false;
                }
            }

            // �z��̐擪���玫���^�ɋl�߂Ă���
            // �C���X�y�N�^�[�Őݒ肵�����Ń��[�h����̂Ŕz��̒��g�����ɑΉ������Ă��Ηǂ�
            for (int i = 0; i < _assetData.Length; i++)
            {
                _assets.Add(_assetData[i].Key, assets[i]);
            }

            // �i���̕\�����~�߂�
            _progress?.Stop();

            return true;
        }
    }
}