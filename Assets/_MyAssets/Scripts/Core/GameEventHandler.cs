using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UnityEngine.Events;

namespace PSB.Ramen
{
    /// <summary>
    /// ゲーム中の各種イベントをハンドリングする。
    /// 継承先がこのクラスで使用している各イベント関数を誤って実装しないように
    /// 予めオーバーライド用のメソッドを用意している。
    /// Awake,Start,OnEnable,OnDisable,OnDestroy
    /// </summary>
    public abstract class GameEventHandler : MonoBehaviour
    {
        // 特定のフラグの有効/無効の切り替えなど、ローカル変数のみで
        // ポーズ処理を完結させるためにコールバックを用いる
        protected static event UnityAction OnPaused;
        protected static event UnityAction OnResumed;

        void Awake()
        {
            MessageBroker.Default.Receive<AssetLoadCompleteMessage>()
                .Subscribe(_ => OnAssetLoadCompleted()).AddTo(gameObject);

            // メッセージ受信でポーズ処理。OnPausedとOnResumed呼ぶ処理ｺｺ

            AwakeOverride();
        }

        void OnEnable()
        {
            OnEnableOverride();
        }

        void OnDisable()
        {
            OnDisableOverride();
        }

        void Start()
        {
            StartOverride();
        }

        void OnDestroy()
        {
            OnDestroyOverride();
        }

        protected virtual void AwakeOverride() { }
        protected virtual void StartOverride() { }
        protected virtual void OnEnableOverride() { }
        protected virtual void OnDisableOverride() { }
        protected virtual void OnDestroyOverride() { }

        protected virtual void OnAssetLoadCompleted() { }
    }
}
