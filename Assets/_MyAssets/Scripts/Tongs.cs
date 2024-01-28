using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Buffers;
using System;

namespace PSB.Ramen
{
    public class Tongs : GameEventHandler
    {
        // �͂�/���������ۂ̃g���O�̊p�x
        const float HoldTongAngle = 0;
        const float ReleasedTongAngle = 15.0f;

        [SerializeField] XRGrabInteractable _interactable;
        [Header("�e��p�����[�^�̐ݒ�")]
        [SerializeField] TongsParameterSettings _settings;
        [Header("���E�̃g���O�̉�]��")]
        [SerializeField] Transform _leftPivot;
        [SerializeField] Transform _rightPivot;
        [Header("�ԐړI�ɒ͂ޔ���")]
        [SerializeField] SphereCollider _grabIndirectly;

        protected override void AwakeOverride()
        {
            RotateTongPivot(ReleasedTongAngle);

            // �͂񂾍ۂɃg���O�̊p�x��ς���
            _interactable.activated.AddListener(_ => RotateTongPivot(HoldTongAngle));
            _interactable.deactivated.AddListener(_ => RotateTongPivot(ReleasedTongAngle));

            // �͂񂾍ۂɋ�ނ�͂�
            _interactable.activated.AddListener(_ => Grab());
            _interactable.deactivated.AddListener(_ => Release());
        }

        void Update()
        {
            if (UnityEngine.InputSystem.Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                Release();
            }
        }

        // �I�u�W�F�N�g�̒[�����ɉ�]���������̂ŕʓr�A���ƂȂ�I�u�W�F�N�g��p�ӂ���
        void RotateTongPivot(float angle)
        {
            if (_leftPivot == null || _rightPivot == null) return;

            _leftPivot.localEulerAngles = new Vector3(0, -angle, 0);
            _rightPivot.localEulerAngles = new Vector3(0, angle, 0);
        }

        // �͂�
        void Grab()
        {
            Collider[] result = Physics.OverlapSphere(_grabIndirectly.transform.position, _grabIndirectly.radius);
            foreach (Collider item in result)
            {
                if (item == null) break;
                if (item.gameObject.TryGetComponent(out Food food))
                {
                    food.Grab(_grabIndirectly.transform);
                    break;
                }
            }
        }

        // �͂�ł������̂𗣂�
        void Release()
        {
            foreach (Transform c in _grabIndirectly.transform)
            {
                if (c == null || !c.gameObject.activeSelf) continue;
                if (c.TryGetComponent(out Food food)) food.Release();
            }

            _grabIndirectly.transform.DetachChildren();
        }
    }
}
