using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PSB.Ramen
{
    /// <summary>
    /// �l�X�ȃN���X�Ŏg�p����R�A�@�\��񋟂���N���X
    /// </summary>
    public class Service : MonoBehaviour
    {
        static Service _service;

        [Header("�V���O���g���Œ񋟂���@�\")]
        [SerializeField] ResourcesAssets _assets;
        [SerializeField] AudioPlayer _audio;

        void Awake()
        {
            if (_service == null)
            {
                _service = this;
            }
            else
            {
                Destroy(this);
            }
        }

        /// <summary>
        /// ���[�h�����A�Z�b�g�𐶐�����
        /// </summary>
        public static GameObject Instantiate(AssetKey key, Vector3 pos = default, 
            Quaternion rot = default, Transform parent = null)
        {
            if (_service._assets == null)
            {
                Debug.LogError("�A�Z�b�g�̐����@�\������");

                // �K���ȋ��̂𐶐����ă_�~�[�Ƃ��ĕԂ�
                GameObject dummy = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                dummy.transform.position = pos;
                return dummy;
            }

            return _service._assets.Instantiate(key, pos, rot, parent);
        }

        /// <summary>
        /// �w�肵��SE��炷
        /// </summary>
        public static void PlaySE(AudioKey key)
        {
            if (_service._audio == null) return;
            _service._audio.PlaySE(key);
        }

        /// <summary>
        /// �w�肵��BGM�𗬂�
        /// </summary>
        public static void PlayBGM(AudioKey key)
        {
            if (_service._audio == null) return;
            _service._audio.PlayBGM(key);
        }
    }
}
