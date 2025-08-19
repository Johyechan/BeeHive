using InGame.MyEnum;
using MyUtil;
using Unity.Cinemachine;
using UnityEngine;

namespace InGame.MyManager
{
    // 작성자: 조혜찬
    // 카메라 매니저 싱글톤 클래스
    public class CameraManager : MonoSingleton<CameraManager>
    {
        [SerializeField] private CinemachineBrain _cameraBrain;

        protected override void Awake()
        {
            base.Awake();

            SetCamera(TeamManager.Instance.CurrentTeamType);
        }

        public void SetCamera(TeamType teamType)
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
        }
    }
}
// 마지막 작성 일자: 2025.08.19