using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PSB.Example
{
    public class AnimationPlayer : MonoBehaviour
    {
        [SerializeField] Animator _animator;

        /// <summary>
        /// �A�j���[�V�����̖��O�𕶎���Ŏw�肵�čĐ�
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
