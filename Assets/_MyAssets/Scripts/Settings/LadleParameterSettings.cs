using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PSB.Ramen
{
    [CreateAssetMenu(fileName = "ParameterSettings_Ladle_", menuName = "ParameterSettings/Ladle")]
    public class LadleParameterSettings : ScriptableObject
    {
        // �I���\�ȕ��݂̂ɍi�����񋓌^��p�ӂ��A�O������擾����ۂɕϊ�����
        enum Key
        {
            Soup,
        }

        [Header("�X�[�v�̃A�Z�b�g�̃L�[")]
        [SerializeField] Key _soupAssetKey;
        [Header("����p�x")]
        [Range(0, 360)]
        [SerializeField] float _angleMinX = 60.0f;
        [Range(0, 360)]
        [SerializeField] float _angleMaxX = 320.0f;
        [Range(0, 360)]
        [SerializeField] float _angleMinZ = 60.0f;
        [Range(0, 360)]
        [SerializeField] float _angleMaxZ = 280.0f;
        [Header("���ʓ��̃X�[�v��")]
        [SerializeField] float _defaultFill = 0;
        [SerializeField] float _reFill = 100;
        [SerializeField] float _flowSpeed = 5.0f;

        public AssetKey SoupAssetKey
        {
            get
            {
                if (_soupAssetKey == Key.Soup) return AssetKey.SoupParticle;
                else return AssetKey.Dummy;
            }
        }
        public float AngleMinX => _angleMinX;
        public float AngleMaxX => _angleMaxX;
        public float AngleMinZ => _angleMinZ;
        public float AngleMaxZ => _angleMaxZ;
        public float DefaultFill => _defaultFill;
        public float ReFill => _reFill;
        public float FlowSpeed => _flowSpeed;
    }
}
