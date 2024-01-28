using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UnityEngine.Events;

namespace PSB.Ramen
{
    /// <summary>
    /// �Q�[�����̊e��C�x���g���n���h�����O����B
    /// �p���悪���̃N���X�Ŏg�p���Ă���e�C�x���g�֐�������Ď������Ȃ��悤��
    /// �\�߃I�[�o�[���C�h�p�̃��\�b�h��p�ӂ��Ă���B
    /// Awake,Start,OnEnable,OnDisable,OnDestroy
    /// </summary>
    public abstract class GameEventHandler : MonoBehaviour
    {
        // ����̃t���O�̗L��/�����̐؂�ւ��ȂǁA���[�J���ϐ��݂̂�
        // �|�[�Y���������������邽�߂ɃR�[���o�b�N��p����
        protected static event UnityAction OnPaused;
        protected static event UnityAction OnResumed;

        void Awake()
        {
            MessageBroker.Default.Receive<AssetLoadCompleteMessage>()
                .Subscribe(_ => OnAssetLoadCompleted()).AddTo(gameObject);

            // ���b�Z�[�W��M�Ń|�[�Y�����BOnPaused��OnResumed�Ăԏ�����

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
