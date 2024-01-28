using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace PSB.Example
{
    [RequireComponent(typeof(Rigidbody))]
    public class Bullet : MonoBehaviour
    {
        [SerializeField] float _power = 30.0f;
        [SerializeField] float _lifeTime = 5.0f;

        CancellationTokenSource _cts = new();

        void OnDestroy()
        {
            _cts?.Cancel();
        }

        /// <summary>
        /// ”ò‚Ô•ûŒü‚ğw’è‚µ‚ÄAˆê’èŠÔŒã‚ÉÁ‚¦‚é’e‚ğ”­Ë
        /// </summary>
        public async UniTask Fire(Vector3 dir)
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.AddForce(dir * _power, ForceMode.Impulse);

            await UniTask.WaitForSeconds(_lifeTime, cancellationToken: _cts.Token);

            Destroy(gameObject);
        }
    }
}
