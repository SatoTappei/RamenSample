using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace PSB.Ramen
{
    /// <summary>
    /// �e��G���[�̃��b�Z�[�W����M���A�\������
    /// </summary>
    public class ErrorPrinter : MonoBehaviour
    {
        [Header("�G���[��\������e�L�X�g")]
        [SerializeField] Text _printText;

        void Awake()
        {
            if (_printText == null)
            {
                Debug.LogWarning("�G���[���b�Z�[�W��\������e�L�X�g������");
                return;
            }

            _printText.text = string.Empty;

            // �V�[���J�n���ɃA�Z�b�g�̃��[�h���s���A���s�����ꍇ��EntryPoint���瑗�M�����
            MessageBroker.Default.Receive<AssetLoadFailureMessage>().Subscribe(_ => 
            {
                _printText.text = "�A�Z�b�g�̃��[�h�Ɏ��s";
            }).AddTo(gameObject);
        }
    }
}
