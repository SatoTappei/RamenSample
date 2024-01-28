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
        // 掴む/放すした際のトングの角度
        const float HoldTongAngle = 0;
        const float ReleasedTongAngle = 15.0f;

        [SerializeField] XRGrabInteractable _interactable;
        [Header("各種パラメータの設定")]
        [SerializeField] TongsParameterSettings _settings;
        [Header("左右のトングの回転軸")]
        [SerializeField] Transform _leftPivot;
        [SerializeField] Transform _rightPivot;
        [Header("間接的に掴む判定")]
        [SerializeField] SphereCollider _grabIndirectly;

        protected override void AwakeOverride()
        {
            RotateTongPivot(ReleasedTongAngle);

            // 掴んだ際にトングの角度を変える
            _interactable.activated.AddListener(_ => RotateTongPivot(HoldTongAngle));
            _interactable.deactivated.AddListener(_ => RotateTongPivot(ReleasedTongAngle));

            // 掴んだ際に具材を掴む
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

        // オブジェクトの端を軸に回転させたいので別途、軸となるオブジェクトを用意する
        void RotateTongPivot(float angle)
        {
            if (_leftPivot == null || _rightPivot == null) return;

            _leftPivot.localEulerAngles = new Vector3(0, -angle, 0);
            _rightPivot.localEulerAngles = new Vector3(0, angle, 0);
        }

        // 掴む
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

        // 掴んでいたものを離す
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
