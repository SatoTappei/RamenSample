using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PSB.Ramen
{
    public class Food : MonoBehaviour, IFood
    {
        [SerializeField] Rigidbody _rigidbody;
        [Header("���[�����ɉ�����ۂ̏��")]
        [SerializeField] FoodType _type;
        [SerializeField] int _volume = 1;

        Transform _prevParent;

        void Awake()
        {
            _prevParent = transform.parent;
        }

        /// <summary>
        /// �e��ݒ肵�ĕ��������𖳌�������
        /// </summary>
        public void Grab(Transform parent)
        {
            _prevParent = transform.parent;
            transform.parent = parent;
            _rigidbody.isKinematic = true;
        }

        /// <summary>
        /// ����������L�������Ē͂�ł����ۂ̐e���猳�̐e�ɖ߂�
        /// </summary>
        public void Release()
        {
            transform.parent = _prevParent;
            _rigidbody.isKinematic = false;
        }

        /// <summary>
        /// ��ނƂ��ėL���ȏꍇ�́u���g�̎�ށv�Ɓu1�񕪂̗ʁv��Ԃ��A���̋�ނ͔j�������
        /// </summary>
        public bool TryAddRamen(out Per<FoodType> food)
        {
            Delete();

            food = new(_type, _volume);
            return true;
        }

        void Delete()
        {
            // Destroy����邱�Ƃɂ��݌v�̔j�]�͖������ꉞ
            gameObject.SetActive(false);
        }
    }
}