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
        // �X�[�v�̔�������Ԋu�ŗ���/�~�߂���s�����߂̃t���O
        class ControlSource
        {
            public float Value;
            public bool Flag;
        }

        [Header("�e��p�����[�^�̐ݒ�")]
        [SerializeField] LadleParameterSettings _settings;
        [Header("�X�[�v�̃p�[�e�B�N���̌���")]
        [SerializeField] Transform _muzzle;
        [Header("���ʓ��փX�[�v�̕�[������s��")]
        [SerializeField] Collider _soopRefillTrigger;
        [Header("�X�[�v�̓����蔻��𗬂��Ԋu")]
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

        // �X�[�v���܂�ɐڐG�������[�����
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

        // �X�[�v�̃A�Z�b�g�ɂ̓p�[�e�B�N�������蓖�Ă��Ă���K�v������
        bool CheckSoupAsset(out ParticleSystem soup)
        {
            AssetKey key = _settings.SoupAssetKey;
            GameObject asset = Service.Instantiate(key, _muzzle.position, parent: transform);
            if (!asset.TryGetComponent(out soup))
            {
                Debug.LogWarning($"ParticleSystem�������̂ŃX�[�v�Ƃ��Ĉ����Ȃ�: {key}");
                Destroy(asset);
                return false;
            }
            return true;
        }

        async UniTask UpdateAsync(ControlSource control, ParticleSystem soup, CancellationToken token)
        {
            // �X����ɂ���ăX�[�v�̔���𗬂�
            FlowSoupCollisionAsync(control, token).Forget();

            Transform t = transform;
            bool isValid = true; // �ꎞ�I�Ȗ������p�̌X���Ă����g���o�����Ȃ�t���O

            while (!token.IsCancellationRequested)
            {
                if (!isValid) return;
                if (control.Value <= 0) { await UniTask.Yield(token); continue; }

                // ���ȏ�X���Ă���ꍇ�̓p�[�e�B�N�����Đ������
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

        // ControlSource�̃t���O��true�̏ꍇ�͈��Ԋu�ŗ���Afalse�̏ꍇ�͎~�܂�
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