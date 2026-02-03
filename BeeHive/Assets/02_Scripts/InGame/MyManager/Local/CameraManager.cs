using InGame.MyEnum;
using MyUtil;
using System.Threading.Tasks;
using Unity.Cinemachine;
using UnityEngine;

namespace InGame.MyManager.Local
{
    // 작성자: 조혜찬
    // 카메라 매니저 클래스
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private CinemachineBrain _cameraBrain;

        public async Task SetCamera(TeamType teamType)
        {
            switch(teamType)
            {
                case TeamType.Team1:
                    _cameraBrain.ChannelMask = OutputChannels.Channel01;
                    break;
                case TeamType.Team2:
                    _cameraBrain.ChannelMask = OutputChannels.Channel02;
                    break;
                case TeamType.Team3:
                    _cameraBrain.ChannelMask = OutputChannels.Channel03;
                    break;
            }

            await Task.CompletedTask;
        }
    }
}
// 마지막 작성 일자: 2026.02.03