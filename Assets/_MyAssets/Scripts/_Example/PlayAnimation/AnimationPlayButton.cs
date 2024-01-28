using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PSB.Example
{
    /// <summary>
    /// アニメーションの再生機能とボタンを紐づける
    /// このクラスが無ければそれぞれが独立した状態になる
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

        // 「待機」と「走る」を交互に再生する
        void SwitchAnimation()
        {
            _currentState = 1 - _currentState;

            if (_currentState == State.Idle) _player.Play(Const.IdleAnimationName);
            if (_currentState == State.Run) _player.Play(Const.RunAnimationName);
        }
    }
}
