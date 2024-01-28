using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace PSB.Ramen
{
    // コントローラ側にRigidbodyが付いていないので、当たり判定を取るために必要
    [RequireComponent(typeof(Rigidbody))]
    public class HandGrabItemStack : GameEventHandler
    {
        [Header("コントローラを検知する")]
        [SerializeField] Collider _collider;
        [Header("積むアイテムのキー")]
        [SerializeField] AssetKey _itemKey;
        
        protected override void AwakeOverride()
        {
            // コントローラもしくは間接的な掴む判定に触れたら生成する
            _collider.OnTriggerEnterAsObservable()
                .Where(c => c.CompareTag(Const.ControllerTag) || c.CompareTag(Const.GrabIndirectlyTag))
                .Subscribe(c => Spawn(c.transform));

            // 初期設定から弄る箇所
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