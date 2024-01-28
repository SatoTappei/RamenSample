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

        [Header("具材として判定される")]
        [SerializeField] FoodType _type;
        [Header("前方向ベクトルの移動速度")]
        [SerializeField] float _uniformSpeed = 0.7f;
        [Header("床の高さまで来たら消す")]
        [SerializeField] float _floorHeight;

        /// <summary>
        /// 生成した側が呼び出す初期化処理
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
                // 自由落下とは違い、前方向のベクトルへ進む
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
            // Destroyされることによる設計の破綻は無いが一応
            gameObject.SetActive(false);
        }
    }
}
