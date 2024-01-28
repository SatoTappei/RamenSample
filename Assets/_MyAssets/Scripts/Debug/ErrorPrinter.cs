using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace PSB.Ramen
{
    /// <summary>
    /// 各種エラーのメッセージを受信し、表示する
    /// </summary>
    public class ErrorPrinter : MonoBehaviour
    {
        [Header("エラーを表示するテキスト")]
        [SerializeField] Text _printText;

        void Awake()
        {
            if (_printText == null)
            {
                Debug.LogWarning("エラーメッセージを表示するテキストが無い");
                return;
            }

            _printText.text = string.Empty;

            // シーン開始時にアセットのロードを行い、失敗した場合にEntryPointから送信される
            MessageBroker.Default.Receive<AssetLoadFailureMessage>().Subscribe(_ => 
            {
                _printText.text = "アセットのロードに失敗";
            }).AddTo(gameObject);
        }
    }
}
