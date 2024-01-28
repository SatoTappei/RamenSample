using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PSB.Ramen
{
    [CreateAssetMenu(fileName = "ParameterSettings_Ladle_", menuName = "ParameterSettings/Ladle")]
    public class LadleParameterSettings : ScriptableObject
    {
        // 選択可能な物のみに絞った列挙型を用意し、外部から取得する際に変換する
        enum Key
        {
            Soup,
        }

        [Header("スープのアセットのキー")]
        [SerializeField] Key _soupAssetKey;
        [Header("零れる角度")]
        [Range(0, 360)]
        [SerializeField] float _angleMinX = 60.0f;
        [Range(0, 360)]
        [SerializeField] float _angleMaxX = 320.0f;
        [Range(0, 360)]
        [SerializeField] float _angleMinZ = 60.0f;
        [Range(0, 360)]
        [SerializeField] float _angleMaxZ = 280.0f;
        [Header("お玉内のスープ量")]
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
