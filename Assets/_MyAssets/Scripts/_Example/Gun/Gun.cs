using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace PSB.Example
{
    public class Gun : MonoBehaviour
    {
        [SerializeField] Transform _muzzle;
        [SerializeField] Bullet _bullet;

        void Start()
        {
            _muzzle ??= transform;
        }

        /// <summary>
        /// 弾を生成、発射し、一定時間後に削除する
        /// </summary>
        public void Fire()
        {
            if (_bullet == null) return;

            Bullet bullet = Instantiate(_bullet);
            bullet.transform.position = _muzzle.position;
            bullet.Fire(_muzzle.forward).Forget();

            Destroy(bullet, 5.0f);
        }
    }
}
