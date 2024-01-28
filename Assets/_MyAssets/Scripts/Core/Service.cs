using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PSB.Ramen
{
    /// <summary>
    /// 様々なクラスで使用するコア機能を提供するクラス
    /// </summary>
    public class Service : MonoBehaviour
    {
        static Service _service;

        [Header("シングルトンで提供する機能")]
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
        /// ロードしたアセットを生成する
        /// </summary>
        public static GameObject Instantiate(AssetKey key, Vector3 pos = default, 
            Quaternion rot = default, Transform parent = null)
        {
            if (_service._assets == null)
            {
                Debug.LogError("アセットの生成機能が無い");

                // 適当な球体を生成してダミーとして返す
                GameObject dummy = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                dummy.transform.position = pos;
                return dummy;
            }

            return _service._assets.Instantiate(key, pos, rot, parent);
        }

        /// <summary>
        /// 指定したSEを鳴らす
        /// </summary>
        public static void PlaySE(AudioKey key)
        {
            if (_service._audio == null) return;
            _service._audio.PlaySE(key);
        }

        /// <summary>
        /// 指定したBGMを流す
        /// </summary>
        public static void PlayBGM(AudioKey key)
        {
            if (_service._audio == null) return;
            _service._audio.PlayBGM(key);
        }
    }
}
