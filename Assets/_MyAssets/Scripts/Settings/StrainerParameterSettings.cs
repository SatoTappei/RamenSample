using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PSB.Ramen
{
    [CreateAssetMenu(fileName = "ParameterSettings_Strainer_", menuName = "ParameterSettings/Strainer")]
    public class StrainerParameterSettings : ScriptableObject
    {
        [Header("U‚é‘¬“x‚ðŒŸ’m‚·‚éƒtƒŒ[ƒ€”")]
        [SerializeField] int _shakeFrameCount = 5;
        [Header("U‚Á‚½‚ÆŒŸ’m‚³‚ê‚éè‡’l")]
        [SerializeField] float _shakeThreshold = 0.1f;
        [Header("U‚Á‚½Û‚É”ò‚Ô…“H")]
        [SerializeField] AssetKey _waterDropAssetKey;
        [SerializeField] float _waterDropInterval = 0.1f;
        [Header("Ž‚¿ã‚°‚½Û‚É”ro‚³‚ê‚é…")]
        [SerializeField] AssetKey _waterAssetKey;
        [SerializeField] float _waterPlayTime = 2.0f;
        [Header("—ë‚ê‚éŠp“x")]
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
