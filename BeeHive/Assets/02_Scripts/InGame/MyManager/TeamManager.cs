using UnityEngine;
using MyUtil;
using TMPro;
using InGame.MyEnum;
using Unity.Android.Gradle;

namespace InGame.MyManager
{
    // 작성자: 조혜찬
    // 서버에서 팀을 배정 받기 위한 싱글톤 클래스
    public class TeamManager : MonoSingleton<TeamManager>
    {
        private TeamType _currentTeamType; // 현재 팀 타입
        // 위에 변수 프로퍼티
        public TeamType CurrentTeamType { get => _currentTeamType; set => _currentTeamType = value; }

        protected override void Awake()
        {
            base.Awake();

            var socket = NetworkManager.Instance.Socket; // 서버와 통신하기 위한 객체 받아오기
            if(socket != null) // 서버와 통신하기 위한 객체가 존재할 경우
            {
                socket.On("teamType", data =>
                {
                    int teamType = data.GetValue<int>(); // int 형으로 전달 받은 값 저장
                    _currentTeamType = (TeamType)teamType; // 팀 저장
                    MainThreadDispatcher.Enqueue(() =>
                    {
                        CameraManager.Instance.SetCamera(_currentTeamType); // 팀 카메라 세팅
                    });
                });
            }
        }
    }
}
// 마지막 작성 일자: 2025.08.19