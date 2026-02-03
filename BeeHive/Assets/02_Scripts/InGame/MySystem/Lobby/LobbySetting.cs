using InGame.MyManager;
using InGame.MyManager.Enum;
using InGame.MyManager.Global;
using TMPro;
using UnityEngine;

namespace InGame.MySystem.Lobby
{
    // 작성자: 조혜찬
    // 로비 세팅 클래스
    public class LobbySetting : MonoBehaviour
    {
        [SerializeField] private TMP_Text _nickNameText; // 닉네임 텍스트

        private void Awake()
        {
            if(SoundManager.Instance.IsFirstStart) // 게임을 이제 처음 시작하는 거라면(로비에 처음 왔다면)
            {
                SoundManager.Instance.IsFirstStart = false; // 이제 처음 시작이 아님
                SoundManager.Instance.SFXPlay(SFXType.BGM); // BGM 실행
            }
        }

        private void Start()
        {
            var socket = NetworkManager.Instance.Socket; // 서버와 통신하기 위한 객체 받아오기

            if(socket != null) // 서버와 통신하기 위한 객체가 null 아닐 때
            {
                //socket.Emit("comeLobby");
            }
        }
    }
}
// 마지막 작성 일자: 2026.02.03