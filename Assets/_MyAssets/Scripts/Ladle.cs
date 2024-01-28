using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using UniRx.Triggers;
using UniRx;

namespace PSB.Ramen
{
    public class Ladle : GameEventHandler
    {
        // スープの判定を一定間隔で流す/止めるを行うためのフラグ
        class ControlSource
        {
            public float Value;
            public bool Flag;
        }

        [Header("各種パラメータの設定")]
        [SerializeField] LadleParameterSettings _settings;
        [Header("スープのパーティクルの源流")]
        [SerializeField] Transform _muzzle;
        [Header("お玉内へスープの補充判定を行う")]
        [SerializeField] Collider _soopRefillTrigger;
        [Header("スープの当たり判定を流す間隔")]
        [SerializeField] float _duration;

        protected override void OnAssetLoadCompleted()
        {
            ControlSource control = new();
            ReFill(control);

            if (CheckSoupAsset(out ParticleSystem soup))
            {
                UpdateAsync(control, soup, this.GetCancellationTokenOnDestroy()).Forget();
            }
        }

        // スープ溜まりに接触したら補充される
        void ReFill(ControlSource control)
        {
            control.Value = _settings.DefaultFill;

            _soopRefillTrigger.OnTriggerEnterAsObservable()
                .Where(c => c.CompareTag(Const.SoopTag))
                .Subscribe(_ => 
                {
                    control.Value = _settings.ReFill;
                }).AddTo(this);
        }

        // スープのアセットにはパーティクルが割り当てられている必要がある
        bool CheckSoupAsset(out ParticleSystem soup)
        {
            AssetKey key = _settings.SoupAssetKey;
            GameObject asset = Service.Instantiate(key, _muzzle.position, parent: transform);
            if (!asset.TryGetComponent(out soup))
            {
                Debug.LogWarning($"ParticleSystemが無いのでスープとして扱えない: {key}");
                Destroy(asset);
                return false;
            }
            return true;
        }

        async UniTask UpdateAsync(ControlSource control, ParticleSystem soup, CancellationToken token)
        {
            // 傾け具合によってスープの判定を流す
            FlowSoupCollisionAsync(control, token).Forget();

            Transform t = transform;
            bool isValid = true; // 一時的な無効化用の傾けても中身が出無くなるフラグ

            while (!token.IsCancellationRequested)
            {
                if (!isValid) return;
                if (control.Value <= 0) { await UniTask.Yield(token); continue; }

                // 一定以上傾いている場合はパーティクルが再生される
                if ((_settings.AngleMinX <= t.eulerAngles.x && t.eulerAngles.x <= _settings.AngleMaxX) ||
                    (_settings.AngleMinZ <= t.eulerAngles.z && t.eulerAngles.z <= _settings.AngleMaxZ))
                {
                    if (!soup.isPlaying) { soup.Play(); control.Flag = true; }
                    
                    control.Value -= _settings.FlowSpeed * Time.deltaTime;
                    control.Value = Mathf.Clamp(control.Value, 0, _settings.ReFill);
                }
                else
                {
                    if (soup.isPlaying){ soup.Stop(); control.Flag = false; }
                }

                await UniTask.Yield(token);
            }
        }

        // ControlSourceのフラグがtrueの場合は一定間隔で流れ、falseの場合は止まる
        async UniTaskVoid FlowSoupCollisionAsync(ControlSource control, CancellationToken token)
        {
            float elapsed = 0;
            while (!token.IsCancellationRequested)
            {
                if (control.Flag) elapsed += Time.deltaTime;

                if (elapsed > _duration)
                {
                    elapsed = 0;
                    GameObject instance = Service.Instantiate(AssetKey.CollisionSoup, _muzzle.position);
                    instance.GetComponent<FreeFallFood>().Init(_muzzle.forward);
                }

                await UniTask.Yield(token);
            }
        }
    }
}