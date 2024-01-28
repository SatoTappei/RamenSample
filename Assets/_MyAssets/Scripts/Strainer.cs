using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using UniRx;
using UniRx.Triggers;

namespace PSB.Ramen
{
    public class Strainer : GameEventHandler
    {
        class ControlSource
        {
            public bool IsEmpty;
        }

        // 手の移動量が小さいので定数値をかけて値を補正する。
        const float DeltaMovement = 1000.0f;

        [SerializeField] Collider _strainer;
        [Header("各種パラメータの設定")]
        [SerializeField] StrainerParameterSettings _settings;
        [Header("麺の生成位置")]
        [SerializeField] Transform _muzzle;
        [Header("水の生成位置")]
        [SerializeField] Transform _bottom;

        protected override void OnAssetLoadCompleted()
        {
            CancellationToken token = this.GetCancellationTokenOnDestroy();
            ControlSource control = new();
            UpdateAsync(token, control).Forget();
            DrainWaterOnExit(control);
            SplashWaterDropOnShakeAsync(token).Forget();
        }

        // 持ち上げた際に水が排出される
        void DrainWaterOnExit(ControlSource control)
        {
            if (!CheckWaterAsset(_settings.WaterAssetKey, out ParticleSystem water)) return;

            CancellationTokenSource cts = new();
            //this.OnDestroyAsObservable().Where(_ => cts != null).Subscribe(_ => cts.Cancel());

            _strainer.OnTriggerExitAsObservable().Where(c => c.CompareTag(Const.WaterTag)).Subscribe(_ =>
            {
                //if (cts != null) cts.Cancel();

                using (cts = new())
                {
                    control.IsEmpty = false;
                    DrainWaterAsync(water, cts.Token).Forget();
                }
            });
        }

        // 振った際に水滴が飛び散る
        async UniTaskVoid SplashWaterDropOnShakeAsync(CancellationToken token)
        {
            if (!CheckWaterAsset(_settings.WaterDropAssetKey, out ParticleSystem waterDrop)) return;

            Queue<float> sum = new();
            Transform t = transform;
            Vector3 prev = t.position;
            while (!token.IsCancellationRequested)
            {
                // 数フレームの間の移動量を保持
                float f = Vector3.SqrMagnitude(prev - t.position) * Time.deltaTime * DeltaMovement;
                sum.Enqueue(f);
                if (sum.Count > _settings.ShakeFrameCount) sum.Dequeue();

                // 一定量移動していた場合は水滴を飛ばす
                if (sum.Sum() > _settings.ShakeThreshold)
                {
                    sum.Clear();
                    waterDrop.Play();
                    await UniTask.WaitForSeconds(_settings.WaterDropInterval, cancellationToken: token);
                }

                prev = t.position;

                await UniTask.Yield(token);
            }
        }

        // 水のアセットにはパーティクルが割り当てられている必要がある
        bool CheckWaterAsset(AssetKey key, out ParticleSystem water)
        {
            GameObject asset = Service.Instantiate(key, _bottom.position, parent: transform);
            if (!asset.TryGetComponent(out water))
            {
                Debug.LogWarning($"ParticleSystemが無いので水として扱えない: {key}");
                Destroy(asset);
                return false;
            }
            return true;
        }

        async UniTask UpdateAsync(CancellationToken token, ControlSource control)
        {
            Transform t = transform;
            bool isValid = true; // 一時的な無効化用の傾けても中身が出無くなるフラグ

            while (!token.IsCancellationRequested)
            {
                if (!isValid) return;

                // 一定以上傾いている場合は麺が生成される
                if ((_settings.AngleMinX <= t.eulerAngles.x && t.eulerAngles.x <= _settings.AngleMaxX) ||
                    (_settings.AngleMinZ <= t.eulerAngles.z && t.eulerAngles.z <= _settings.AngleMaxZ))
                {
                    if (!control.IsEmpty)
                    {
                        control.IsEmpty = true;
                        CreateNoodle();
                    }
                }

                await UniTask.Yield(token);
            }
        }

        // 麺を生成
        void CreateNoodle()
        {
            GameObject asset = Service.Instantiate(AssetKey.NoodleObject, _muzzle.position);
            if (asset.TryGetComponent(out FreeFallFood food))
            {
                food.Init(_muzzle.forward);
            }
            else
            {
                Debug.LogWarning($"ざるから飛ばす麺として対応していない: {asset.name}");
            }
        }

        // 一定時間水が排出される
        async UniTask DrainWaterAsync(ParticleSystem water, CancellationToken token)
        {
            token.Register(() => water.Stop());
            water.Play();
            await UniTask.WaitForSeconds(_settings.WaterPlayTime, cancellationToken: token);
            water.Stop();
        }
    }
}
