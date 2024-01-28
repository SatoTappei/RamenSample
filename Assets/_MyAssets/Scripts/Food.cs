using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PSB.Ramen
{
    public class Food : MonoBehaviour, IFood
    {
        [SerializeField] Rigidbody _rigidbody;
        [Header("ラーメンに加える際の情報")]
        [SerializeField] FoodType _type;
        [SerializeField] int _volume = 1;

        Transform _prevParent;

        void Awake()
        {
            _prevParent = transform.parent;
        }

        /// <summary>
        /// 親を設定して物理挙動を無効化する
        /// </summary>
        public void Grab(Transform parent)
        {
            _prevParent = transform.parent;
            transform.parent = parent;
            _rigidbody.isKinematic = true;
        }

        /// <summary>
        /// 物理挙動を有効化して掴んでいた際の親から元の親に戻す
        /// </summary>
        public void Release()
        {
            transform.parent = _prevParent;
            _rigidbody.isKinematic = false;
        }

        /// <summary>
        /// 具材として有効な場合は「自身の種類」と「1回分の量」を返し、この具材は破棄される
        /// </summary>
        public bool TryAddRamen(out Per<FoodType> food)
        {
            Delete();

            food = new(_type, _volume);
            return true;
        }

        void Delete()
        {
            // Destroyされることによる設計の破綻は無いが一応
            gameObject.SetActive(false);
        }
    }
}