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

        // ��̈ړ��ʂ��������̂Œ萔�l�������Ēl��␳����B
        const float DeltaMovement = 1000.0f;

        [SerializeField] Collider _strainer;
        [Header("�e��p�����[�^�̐ݒ�")]
        [SerializeField] StrainerParameterSettings _settings;
        [Header("�˂̐����ʒu")]
        [SerializeField] Transform _muzzle;
        [Header("���̐����ʒu")]
        [SerializeField] Transform _bottom;

        protected override void OnAssetLoadCompleted()
        {
            CancellationToken token = this.GetCancellationTokenOnDestroy();
            ControlSource control = new();
            UpdateAsync(token, control).Forget();
            DrainWaterOnExit(control);
            SplashWaterDropOnShakeAsync(token).Forget();
        }

        // �����グ���ۂɐ����r�o�����
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

        // �U�����ۂɐ��H����юU��
        async UniTaskVoid SplashWaterDropOnShakeAsync(CancellationToken token)
        {
            if (!CheckWaterAsset(_settings.WaterDropAssetKey, out ParticleSystem waterDrop)) return;

            Queue<float> sum = new();
            Transform t = transform;
            Vector3 prev = t.position;
            while (!token.IsCancellationRequested)
            {
                // ���t���[���̊Ԃ̈ړ��ʂ�ێ�
                float f = Vector3.SqrMagnitude(prev - t.position) * Time.deltaTime * DeltaMovement;
                sum.Enqueue(f);
                if (sum.Count > _settings.ShakeFrameCount) sum.Dequeue();

                // ���ʈړ����Ă����ꍇ�͐��H���΂�
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

        // ���̃A�Z�b�g�ɂ̓p�[�e�B�N�������蓖�Ă��Ă���K�v������
        bool CheckWaterAsset(AssetKey key, out ParticleSystem water)
        {
            GameObject asset = Service.Instantiate(key, _bottom.position, parent: transform);
            if (!asset.TryGetComponent(out water))
            {
                Debug.LogWarning($"ParticleSystem�������̂Ő��Ƃ��Ĉ����Ȃ�: {key}");
                Destroy(asset);
                return false;
            }
            return true;
        }

        async UniTask UpdateAsync(CancellationToken token, ControlSource control)
        {
            Transform t = transform;
            bool isValid = true; // �ꎞ�I�Ȗ������p�̌X���Ă����g���o�����Ȃ�t���O

            while (!token.IsCancellationRequested)
            {
                if (!isValid) return;

                // ���ȏ�X���Ă���ꍇ�͖˂����������
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

        // �˂𐶐�
        void CreateNoodle()
        {
            GameObject asset = Service.Instantiate(AssetKey.NoodleObject, _muzzle.position);
            if (asset.TryGetComponent(out FreeFallFood food))
            {
                food.Init(_muzzle.forward);
            }
            else
            {
                Debug.LogWarning($"���邩���΂��˂Ƃ��đΉ����Ă��Ȃ�: {asset.name}");
            }
        }

        // ��莞�Ԑ����r�o�����
        async UniTask DrainWaterAsync(ParticleSystem water, CancellationToken token)
        {
            token.Register(() => water.Stop());
            water.Play();
            await UniTask.WaitForSeconds(_settings.WaterPlayTime, cancellationToken: token);
            water.Stop();
        }
    }
}
