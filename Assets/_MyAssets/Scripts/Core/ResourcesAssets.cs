using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace PSB.Ramen
{
    /// <summary>
    /// ���̃V�[���Ŏg�p���铮�I�ɐ��������A�Z�b�g��񓯊��Ń��[�h���A�ێ�����
    /// ���[�h�����A�Z�b�g�͗񋓌^�̃L�[���w�肵�Ď擾�ł���
    /// �r���h���Addressable��p�������[�h����肭�����Ȃ��̂ŁAResources��p�����������[�h���s��
    /// </summary>
    public class ResourcesAssets : MonoBehaviour
    {
        [System.Serializable]
        public class AssetData
        {
            [field: SerializeField] public string Name { get; private set; }
            [field: SerializeField] public AssetKey Key { get; private set; }
        }

        // �P�̂œ��삳���邽�߁AStart���Ɏ����Ń��[�h����t���O
        // �������ςȂ��Ȃ̂ŏI����҂��Ȃ�
        readonly bool LoadOnStart = false;

        [Header("���[�h����A�Z�b�g�̃f�[�^")]
        [SerializeField] AssetData[] _assetData;
        [Header("�i����\������UI")]
        [SerializeField] LoadProgress _progress;

        Dictionary<AssetKey, GameObject> _assets = new();

        /// <summary>
        /// �A�Z�b�g�̃��[�h���ς݁A�Q�Ƃ␶�����\�ȏ�ԂɂȂ����t���O
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
        /// �ǂݍ��񂾃A�Z�b�g���擾����
        /// </summary>
        public GameObject Get(AssetKey key)
        {
            if (!IsLoaded)
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
            if (IsLoaded)
            {
                Debug.LogWarning("���ɃA�Z�b�g�����[�h�ς�");
                return false;
            }
            IsLoaded = true;

            // �i���̕\��
            _progress?.Play();

            foreach(AssetData data in _assetData)
            {
                // Resources��p�����񓯊����[�h
                ResourceRequest request = Resources.LoadAsync<GameObject>(data.Name);
                await UniTask.WaitUntil(() => request.isDone, cancellationToken: token);

                GameObject asset = request.asset as GameObject;
                if (asset == null)
                {
                    throw new System.NullReferenceException("�A�Z�b�g�̃��[�h�Ɏ��s: " + data.Key);
                }

                _assets.Add(data.Key, asset);
            }

            await UniTask.WaitForSeconds(1.0f, cancellationToken: token);

            // �i���̕\�����~�߂�
            _progress?.Stop();

            return true;
        }
    }
}
