using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace PSB.Ramen
{
    public class FreeFallFood : MonoBehaviour, IFood
    {
        const float FallSpeedMax = 9.8f;
        const float Gravity = 1.8f;

        [Header("��ނƂ��Ĕ��肳���")]
        [SerializeField] FoodType _type;
        [Header("�O�����x�N�g���̈ړ����x")]
        [SerializeField] float _uniformSpeed = 0.7f;
        [Header("���̍����܂ŗ��������")]
        [SerializeField] float _floorHeight;

        /// <summary>
        /// �������������Ăяo������������
        /// </summary>
        public void Init(Vector3 forward)
        {
            UpdateAsync(forward, this.GetCancellationTokenOnDestroy()).Forget();
        }

        async UniTaskVoid UpdateAsync(Vector3 forward, CancellationToken token)
        {
            Transform transform = this.transform;
            float acc = 1;

            while (!token.IsCancellationRequested && transform.position.y >= _floorHeight)
            {
                // ���R�����Ƃ͈Ⴂ�A�O�����̃x�N�g���֐i��
                transform.Translate(forward * Time.deltaTime * _uniformSpeed);

                float y = 0.5f * Gravity * acc * acc;
                transform.Translate(Vector3.down * y * Time.deltaTime);

                acc += Time.deltaTime;
                acc = Mathf.Min(acc, FallSpeedMax);

                await UniTask.Yield(token);
            }

            Delete();
        }

        public bool TryAddRamen(out Per<FoodType> food)
        {
            Delete();

            food = new(_type, 1);
            return true;
        }

        void Delete()
        {
            // Destroy����邱�Ƃɂ��݌v�̔j�]�͖������ꉞ
            gameObject.SetActive(false);
        }
    }
}
