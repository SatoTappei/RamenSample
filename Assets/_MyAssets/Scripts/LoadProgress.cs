using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using UniRx;
using UniRx.Triggers;

namespace PSB.Ramen
{
    public class LoadProgress : MonoBehaviour
    {
        [Header("進捗表示UI")]
        [SerializeField] Transform _progressText;
        [SerializeField] Transform _icon;
        [Header("回転速度")]
        [SerializeField] float _rotSpeed = 1.0f;

        CancellationTokenSource _cts;
        Vector3 _defaultProgressTextScale;
        Vector3 _defaultIconScale;

        void Awake()
        {
            _defaultProgressTextScale = _progressText.localScale;
            _defaultIconScale = _icon.localScale;

            Stop();
            this.OnDestroyAsObservable().Subscribe(_ => _cts?.Cancel());
        }

        /// <summary>
        /// UIを表示し、回転アニメーションを再生する
        /// Stopを呼び出して止める
        /// </summary>
        public void Play()
        {
            if (_icon == null) return;

            _progressText.localScale = _defaultProgressTextScale;
            _icon.localScale = _defaultIconScale;
            _icon.localEulerAngles = Vector3.zero;
            _cts = new();
            PlayAsync(_cts.Token).Forget();
        }

        async UniTaskVoid PlayAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                _icon.Rotate(new Vector3(0, 0, _rotSpeed));
                await UniTask.Yield(token);
            }
        }

        /// <summary>
        /// 回転アニメーションを止め、UIを非表示にする
        /// </summary>
        public void Stop()
        {
            if (_icon == null) return;

            if (_cts != null) _cts.Cancel();
            _progressText.localScale = Vector3.zero;
            _icon.localScale = Vector3.zero;
        }
    }
}