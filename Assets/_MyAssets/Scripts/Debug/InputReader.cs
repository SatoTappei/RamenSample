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
        [Header("�R���g���[��������͂��ꂽ�l��\��")]
        [SerializeField] Text _controllerValueText;

        List<InputDevice> _devices = new List<InputDevice>();

        void Start()
        {
            CancellationToken token = this.GetCancellationTokenOnDestroy();
            CheckDeviceAsync(token).Forget();
            CheckControllerValueAsync(token).Forget();
        }

        // �K�v�ȓ��̓f�o�C�X���ڑ������܂ŌJ��Ԃ����ׂ�
        async UniTask CheckDeviceAsync(CancellationToken token)
        {
            // ���E�̃R���g���[�����K�v
            while (_devices.Count < 2)
            {
                InputDevices.GetDevices(_devices);
                _devices.ForEach(d => Debug.Log($"{d.name} {d.characteristics}"));

                await UniTask.Yield(token);
            }
        }

        // ���E�̃R���g���[���̓��͂��J��Ԃ����ׂ�
        async UniTask CheckControllerValueAsync(CancellationToken token)
        {
            if (_controllerValueText == null) return;

            StringBuilder str = new();
            while (true)
            {
                InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Left | 
                    InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller, _devices);

                // �R���g���[�����̓��͂𕶎���ɂ���1�s���ǉ����Ă���
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
