using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PSB.Example
{
    public class AnimationPlayer : MonoBehaviour
    {
        [SerializeField] Animator _animator;

        /// <summary>
        /// アニメーションの名前を文字列で指定して再生
        /// </summary>
        public void Play(string name)
        {
            if (_animator != null)
            {
                _animator.Play(name);
            }
        }
    }
}
