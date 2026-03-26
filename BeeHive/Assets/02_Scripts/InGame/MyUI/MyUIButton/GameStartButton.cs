using InGame.MyManager;
using InGame.MyManager.Global;
using InGame.MyUI.MyUIInterface;
using UnityEngine;
using UnityEngine.EventSystems;

namespace InGame.MyUI.MyUIButton
{
    // 작성자: 조혜찬
    // 게임 시작 버튼 클래스
    public class GameStartButton : MonoBehaviour, IUIClick
    {
        // 클릭 시 실행될 함수
        public void OnUIClick()
        {
            var socket = NetworkManager.Instance.Socket; // 서버와 통신하기 위한 객체 받아오기
            if(socket != null) // 서버와 통신하는 객체가 null이 아닐경우
            {
                if(SceneMgr.Instance.CurrentRoomID != "") // 현재 방이 있을 경우
                {
                    socket.Emit("gameStart", SceneMgr.Instance.CurrentRoomID);
                }
            }
            EventSystem.current.SetSelectedGameObject(null); // 선택한 객체 초기화
        }
    }
}
// 마지막 작성 일자: 2026.03.26