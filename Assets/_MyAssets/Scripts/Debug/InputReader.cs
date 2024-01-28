using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine.UI;
using System.Text;

namespace PSB
{
    public class InputReader : MonoBehaviour
    {
        [Header("コントローラから入力された値を表示")]
        [SerializeField] Text _controllerValueText;

        List<InputDevice> _devices = new List<InputDevice>();

        void Start()
        {
            CancellationToken token = this.GetCancellationTokenOnDestroy();
            CheckDeviceAsync(token).Forget();
            CheckControllerValueAsync(token).Forget();
        }

        // 必要な入力デバイスが接続されるまで繰り返し調べる
        async UniTask CheckDeviceAsync(CancellationToken token)
        {
            // 左右のコントローラが必要
            while (_devices.Count < 2)
            {
                InputDevices.GetDevices(_devices);
                _devices.ForEach(d => Debug.Log($"{d.name} {d.characteristics}"));

                await UniTask.Yield(token);
            }
        }

        // 左右のコントローラの入力を繰り返し調べる
        async UniTask CheckControllerValueAsync(CancellationToken token)
        {
            if (_controllerValueText == null) return;

            StringBuilder str = new();
            while (true)
            {
                InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Left | 
                    InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller, _devices);

                // コントローラ毎の入力を文字列にして1行ずつ追加していく
                _devices.ForEach(d => 
                {
                    d.TryGetFeatureValue(CommonUsages.trigger, out float value);
                    str.AppendLine($"{d.name} {value}");
                });

                _controllerValueText.text = str.ToString();
                str.Clear();

                await UniTask.Yield(token);
            }
        }
    }
}
