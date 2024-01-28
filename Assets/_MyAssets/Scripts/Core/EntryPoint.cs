using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using UniRx;
using UniRx.Triggers;

namespace PSB.Ramen
{
    // 設計
    // このクラスがゲーム自体の流れを管理し、イベントをメッセージングする。
    // 投げっぱなしの処理はメッセージングで行っている。
    // Instantiateするオブジェクトは○○Assetsクラスが管理しており、生成して返してくれる。
    // 生成や音の再生などのコア機能はシングルトンのServiceクラスが持っている。

    public struct GameStartMessage { }
    public struct AssetLoadCompleteMessage { }
    public struct AssetLoadFailureMessage { }
    public struct GameOverMessage { }

    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] ResourcesAssets _assets;

        void Start()
        {
            ExecuteAsync(this.GetCancellationTokenOnDestroy()).Forget();
        }

        async UniTaskVoid ExecuteAsync(CancellationToken token)
        {
            bool loadSuccess = await _assets.LoadAllAsync(token);
            if (!loadSuccess)
            {
                // アセットのロードに失敗した場合はメッセージを送信する
                MessageBroker.Default.Publish(new AssetLoadFailureMessage());
            }

            // 一応、アセットのロードが完了してから次のUpdateのタイミングまで待つ
            await UniTask.Yield(PlayerLoopTiming.Update, token);
            MessageBroker.Default.Publish(new AssetLoadCompleteMessage());
        }
    }
}