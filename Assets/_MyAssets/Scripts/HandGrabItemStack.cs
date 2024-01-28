using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace PSB.Ramen
{
    // �R���g���[������Rigidbody���t���Ă��Ȃ��̂ŁA�����蔻�����邽�߂ɕK�v
    [RequireComponent(typeof(Rigidbody))]
    public class HandGrabItemStack : GameEventHandler
    {
        [Header("�R���g���[�������m����")]
        [SerializeField] Collider _collider;
        [Header("�ςރA�C�e���̃L�[")]
        [SerializeField] AssetKey _itemKey;
        
        protected override void AwakeOverride()
        {
            // �R���g���[���������͊ԐړI�Ȓ͂ޔ���ɐG�ꂽ�琶������
            _collider.OnTriggerEnterAsObservable()
                .Where(c => c.CompareTag(Const.ControllerTag) || c.CompareTag(Const.GrabIndirectlyTag))
                .Subscribe(c => Spawn(c.transform));

            // �����ݒ肩��M��ӏ�
            GetComponent<Rigidbody>().useGravity = false;
            _collider.isTrigger = true;
        }

        void Spawn(Transform controller)
        {
            Vector3 spawnPos = transform.position;
            spawnPos.y = controller.position.y;
            
            Service.Instantiate(_itemKey, spawnPos);
        }
    }
}