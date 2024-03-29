using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PSB.Ramen
{
    [CreateAssetMenu(fileName = "ParameterSettings_Strainer_", menuName = "ParameterSettings/Strainer")]
    public class StrainerParameterSettings : ScriptableObject
    {
        [Header("振る速度を検知するフレーム数")]
        [SerializeField] int _shakeFrameCount = 5;
        [Header("振ったと検知される閾値")]
        [SerializeField] float _shakeThreshold = 0.1f;
        [Header("振った際に飛ぶ水滴")]
        [SerializeField] AssetKey _waterDropAssetKey;
        [SerializeField] float _waterDropInterval = 0.1f;
        [Header("持ち上げた際に排出される水")]
        [SerializeField] AssetKey _waterAssetKey;
        [SerializeField] float _waterPlayTime = 2.0f;
        [Header("零れる角度")]
        [Range(0, 360)]
        [SerializeField] float _angleMinX = 60.0f;
        [Range(0, 360)]
        [SerializeField] float _angleMaxX = 320.0f;
        [Range(0, 360)]
        [SerializeField] float _angleMinZ = 60.0f;
        [Range(0, 360)]
        [SerializeField] float _angleMaxZ = 280.0f;

        public AssetKey WaterDropAssetKey => _waterDropAssetKey;
        public AssetKey WaterAssetKey => _waterAssetKey;
        public float AngleMinX => _angleMinX;
        public float AngleMaxX => _angleMaxX;
        public float AngleMinZ => _angleMinZ;
        public float AngleMaxZ => _angleMaxZ;
        public float WaterPlayTime => _waterPlayTime;
        public int ShakeFrameCount => _shakeFrameCount;
        public float ShakeThreshold => _shakeThreshold;
        public float WaterDropInterval => _waterDropInterval;
    }
}
