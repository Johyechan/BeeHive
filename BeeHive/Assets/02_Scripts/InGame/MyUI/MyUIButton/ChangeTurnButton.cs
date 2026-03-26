using InGame.MyEnum;
using InGame.MyEvent;
using InGame.MyManager;
using InGame.MyManager.Global;
using InGame.MyManager.Local;
using InGame.MyUI.MyUIInterface;
using MyUtil.GameMode;
using Tutorial;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace InGame.MyUI.MyUIButton
{
    // 작성자: 조혜찬
    // 턴 변경 버튼 클래스
    public class ChangeTurnButton : MonoBehaviour, IUIClick
    {
        private Button _button; // 현재 버튼

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            TurnEvents.OnSetInteractable += SetInteractable;
        }

        private void OnDisable()
        {
            TurnEvents.OnSetInteractable -= SetInteractable;
        }

        private void SetInteractable(bool interactable)
        {
            _button.interactable = interactable;
        }

        // 클릭 시 실행될 함수
        public void OnUIClick()
        {
            var socket = NetworkManager.Instance.Socket; // 서버와 통신하기 위한 객체 받아오기

            if (socket != null) // 서버와 통신하기 위한 객체가 존재할 때
            {
                if(!UIManager.Instance.CanInteractionUI) // UI 상호작용 불가일 때 
                {
                    if (GameModeManager.Instance.CurrentGameMode.UseServer())
                        NetworkManager.Instance.Socket.Emit("debug", "UI 상호 작용 안됨");
                    EventSystem.current.SetSelectedGameObject(null); // 선택한 객체 초기화
                    return; // 반환
                }
                if (!InGameContext.Current.Data.TurnManager.CanChangeTurn) // 턴 변경 가능 상태가 아닐 경우
                {
                    if (GameModeManager.Instance.CurrentGameMode.UseServer())
                        NetworkManager.Instance.Socket.Emit("debug", "턴 변경 가능 상태 아님");
                    EventSystem.current.SetSelectedGameObject(null); // 선택한 객체 초기화
                    return; // 반환
                }

                if(InGameContext.Current.Data.TurnManager.CurrentTeamType == TeamManager.Instance.CurrentTeamType) // 현재 턴의 팀이 내 팀일 경우
                {
                    if(GameModeManager.Instance.CurrentGameMode.UseServer()) // 서버를 사용하는 게임 모드라면
                    {
                        if (GameModeManager.Instance.CurrentGameMode.UseServer())
                            NetworkManager.Instance.Socket.Emit("turnTimerStop", SceneMgr.Instance.CurrentRoomID); // 턴 타이머 종료
                    }
                    else // 서버를 사용하지 않는 게임 모드라면
                    {
                        TutorialManager.Instance.SetTutorialPanel(false);

                        switch (InGameContext.Current.Data.TurnManager.CurrentTurnType) // 현재 턴이
                        {
                            case TurnType.DrawTurn: // 드로우 턴일 때
                                _ = InGameContext.Current.Data.TurnManager.NextTurn(TurnType.MainTurn); // 메인 턴으로 턴 변경
                                break;
                            case TurnType.MainTurn: // 메인 턴일 때
                                _ = InGameContext.Current.Data.TurnManager.NextTurn(TurnType.TurnEnd); // 턴 종료 턴으로 턴 변경
                                break;
                        }
                    }
                }
            }
            EventSystem.current.SetSelectedGameObject(null); // 선택한 객체 초기화
        }
    }
}
// 마지막 작성 일자: 2026.03.26