using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Text;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace PSB.Example
{
    /// <summary>
    /// InputSystemでキーボードからの入力を試す
    /// </summary>
    public class KeyboardInput : MonoBehaviour
    {
        [Header("入力された文字列を表示する")]
        [SerializeField] Text _printText;

        void Start()
        {
            _printText.text = string.Empty;
            LoopAsync(this.GetCancellationTokenOnDestroy()).Forget();
        }

        async UniTaskVoid LoopAsync(CancellationToken token)
        {
            StringBuilder stringBuilder = new();
            while (!token.IsCancellationRequested)
            {
                // 入力があった際にテキストに反映する
                if (Input(out string letter))
                {
                    stringBuilder.Append(letter);
                    _printText.text = stringBuilder.ToString();
                }

                // エンターキーが押された場合は文字をクリアする
                if (Keyboard.current.enterKey.wasPressedThisFrame)
                {
                    stringBuilder.Clear();
                    _printText.text = stringBuilder.ToString();
                }

                await UniTask.Yield(token);
            }
        }

        bool Input(out string letter)
        {
            if (Keyboard.current.aKey.wasPressedThisFrame) { letter = "A"; return true; }
            else if (Keyboard.current.bKey.wasPressedThisFrame) { letter = "B"; return true; }
            else if (Keyboard.current.cKey.wasPressedThisFrame) { letter = "C"; return true; }
            else if (Keyboard.current.dKey.wasPressedThisFrame) { letter = "D"; return true; }
            else if (Keyboard.current.eKey.wasPressedThisFrame) { letter = "E"; return true; }
            else if (Keyboard.current.fKey.wasPressedThisFrame) { letter = "F"; return true; }
            else if (Keyboard.current.gKey.wasPressedThisFrame) { letter = "G"; return true; }
            else if (Keyboard.current.hKey.wasPressedThisFrame) { letter = "H"; return true; }
            else if (Keyboard.current.iKey.wasPressedThisFrame) { letter = "I"; return true; }
            else if (Keyboard.current.jKey.wasPressedThisFrame) { letter = "J"; return true; }
            else if (Keyboard.current.kKey.wasPressedThisFrame) { letter = "K"; return true; }
            else if (Keyboard.current.lKey.wasPressedThisFrame) { letter = "L"; return true; }
            else if (Keyboard.current.mKey.wasPressedThisFrame) { letter = "M"; return true; }
            else if (Keyboard.current.nKey.wasPressedThisFrame) { letter = "N"; return true; }
            else if (Keyboard.current.oKey.wasPressedThisFrame) { letter = "O"; return true; }
            else if (Keyboard.current.pKey.wasPressedThisFrame) { letter = "P"; return true; }
            else if (Keyboard.current.qKey.wasPressedThisFrame) { letter = "Q"; return true; }
            else if (Keyboard.current.rKey.wasPressedThisFrame) { letter = "R"; return true; }
            else if (Keyboard.current.sKey.wasPressedThisFrame) { letter = "S"; return true; }
            else if (Keyboard.current.tKey.wasPressedThisFrame) { letter = "T"; return true; }
            else if (Keyboard.current.uKey.wasPressedThisFrame) { letter = "U"; return true; }
            else if (Keyboard.current.vKey.wasPressedThisFrame) { letter = "V"; return true; }
            else if (Keyboard.current.wKey.wasPressedThisFrame) { letter = "W"; return true; }
            else if (Keyboard.current.xKey.wasPressedThisFrame) { letter = "X"; return true; }
            else if (Keyboard.current.yKey.wasPressedThisFrame) { letter = "Y"; return true; }
            else if (Keyboard.current.zKey.wasPressedThisFrame) { letter = "Z"; return true; }

            letter = string.Empty;
            return false;
        }
    }
}