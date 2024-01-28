using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PSB.Utility
{
    public class Profiler : MonoBehaviour
    {
        [SerializeField] Text _text;
        [SerializeField] float _interval = 0.5f;

        float _timeCount;
        float _timer;
        int _frameCount;

        void Start()
        {
            if (_text == null)
            {
                enabled = false;
            }
            else
            {
                _text.text = string.Empty;
            }
        }

        void Update()
        {
            _timer += Time.deltaTime;
            _timeCount += Time.timeScale / Time.deltaTime;
            _frameCount++;

            if (_timer > _interval)
            {
                _timer = 0;

                // FPS
                float fps = _timeCount / _frameCount;
                _timeCount = 0;
                _frameCount = 0;
                // ÉÅÉÇÉä
                float used = UnityEngine.Profiling.Profiler.GetTotalAllocatedMemoryLong() / (1024.0f * 1000);
                float unUsed = UnityEngine.Profiling.Profiler.GetTotalUnusedReservedMemoryLong() / (1024.0f * 1000);
                float total = UnityEngine.Profiling.Profiler.GetTotalReservedMemoryLong() / (1024.0f * 1000);

                _text.text = $"FPS: {fps.ToString("F2")}\n" +
                             $"Used: {used.ToString("F2")}\n" +
                             $"UnUsed:{unUsed.ToString("F2")}\n" +
                             $"Total: {total.ToString("F2")}";
            }
        }
    }
}
