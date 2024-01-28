using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using UniRx;
using UniRx.Triggers;

namespace PSB.Ramen
{
    // �݌v
    // ���̃N���X���Q�[�����̗̂�����Ǘ����A�C�x���g�����b�Z�[�W���O����B
    // �������ςȂ��̏����̓��b�Z�[�W���O�ōs���Ă���B
    // Instantiate����I�u�W�F�N�g�́���Assets�N���X���Ǘ����Ă���A�������ĕԂ��Ă����B
    // �����≹�̍Đ��Ȃǂ̃R�A�@�\�̓V���O���g����Service�N���X�������Ă���B

    public struct GameStartMessage { }
    public struct AssetLoadCompleteMessage { }
    public struct AssetLoadFailureMessage { }
    public struct GameOverMessage { }

    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] ResourcesAssets _assets;

        void Start()
        {
            ExecuteAsync(this.GetCancellationTokenOnDestroy()).Forget();
        }

        async UniTaskVoid ExecuteAsync(CancellationToken token)
        {
            bool loadSuccess = await _assets.LoadAllAsync(token);
            if (!loadSuccess)
            {
                // �A�Z�b�g�̃��[�h�Ɏ��s�����ꍇ�̓��b�Z�[�W�𑗐M����
                MessageBroker.Default.Publish(new AssetLoadFailureMessage());
            }

            // �ꉞ�A�A�Z�b�g�̃��[�h���������Ă��玟��Update�̃^�C�~���O�܂ő҂�
            await UniTask.Yield(PlayerLoopTiming.Update, token);
            MessageBroker.Default.Publish(new AssetLoadCompleteMessage());
        }
    }
}