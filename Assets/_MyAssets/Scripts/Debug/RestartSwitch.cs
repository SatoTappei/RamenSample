using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UnityEngine.SceneManagement;

namespace PSB.Ramen
{
    public class RestartSwitch : MonoBehaviour
    {
        [SerializeField] Collider _trigger;

        void Start()
        {
            _trigger.OnTriggerEnterAsObservable()
                .Where(c => c.CompareTag(Const.ControllerTag))
                .Subscribe(_ => SceneManager.LoadScene(SceneManager.GetActiveScene().name));
        }
    }
}
