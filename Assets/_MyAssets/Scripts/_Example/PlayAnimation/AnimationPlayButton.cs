using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PSB.Example
{
    /// <summary>
    /// �A�j���[�V�����̍Đ��@�\�ƃ{�^����R�Â���
    /// ���̃N���X��������΂��ꂼ�ꂪ�Ɨ�������ԂɂȂ�
    /// </summary>
    public class AnimationPlayButton : MonoBehaviour
    {
        enum State
        {
            Idle,
            Run,
        }

        [SerializeField] Button _button;
        [SerializeField] AnimationPlayer _player;

        State _currentState = State.Idle;

        void Start()
        {
            _button.onClick.AddListener(SwitchAnimation);
        }

        // �u�ҋ@�v�Ɓu����v�����݂ɍĐ�����
        void SwitchAnimation()
        {
            _currentState = 1 - _currentState;

            if (_currentState == State.Idle) _player.Play(Const.IdleAnimationName);
            if (_currentState == State.Run) _player.Play(Const.RunAnimationName);
        }
    }
}
