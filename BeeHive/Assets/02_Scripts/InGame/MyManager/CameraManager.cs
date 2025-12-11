using InGame.MyEnum;
using MyUtil;
using System.Threading.Tasks;
using Unity.Cinemachine;
using UnityEngine;

namespace InGame.MyManager
{
    // 작성자: 조혜찬
    // 카메라 매니저 싱글톤 클래스
    public class CameraManager : MonoSingleton<CameraManager>
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
// 마지막 작성 일자: 2025.12.11