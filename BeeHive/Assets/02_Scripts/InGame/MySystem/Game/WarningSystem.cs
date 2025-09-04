using InGame.MyEnum;
using InGame.MyEvent;
using InGame.MyManager;
using UnityEngine;

namespace InGame.MySystem
{
    // 작성자: 조혜찬
    // 경고 상황 확인 및 경고 상황일 경우 UI를 띄우는 시스템 클래스
    public class WarningSystem : MonoBehaviour
    {
        private void OnEnable()
        {
            WarningEvent.OnCheckCurrentTurn += IsCurrentTurn; // 현재 턴을 확인하는 델리게이트에 구독
            WarningEvent.OnCanPayCost += CanPayCost; // 비용을 지불할 수 있는 확인하는 델리게이트에 구독
            WarningEvent.OnCheckLeftPieceCount += CheckLeftPieceCount; // 자식 수를 통해서 남은 기물 수를 확인하는 델리게이트에 구독
            WarningEvent.OnCheckCurrentTurnTeam += CheckCurrentTurnTeam; // 현재 턴의 팀을 확인하는 델리게이트에 구독
        }

        private void OnDisable()
        {
            WarningEvent.OnCheckCurrentTurn -= IsCurrentTurn; // 현재 턴을 확인하는 델리게이트에 구독 해제
            WarningEvent.OnCanPayCost -= CanPayCost; // 비용을 지불할 수 있는 확인하는 델리게이트에 구독 해제
            WarningEvent.OnCheckLeftPieceCount -= CheckLeftPieceCount; // 자식 수를 통해서 남은 기물 수를 확인하는 델리게이트에 구독 해제
            WarningEvent.OnCheckCurrentTurnTeam -= CheckCurrentTurnTeam; // 현재 턴의 팀을 확인하는 델리게이트에 구독 해제
        }

        // 현재 턴을 확인하는 함수
        private bool IsCurrentTurn(TurnType currentTurn, string text)
        {
            if (currentTurn != TurnManager.Instance.CurrentTurnType) // 확인하려는 턴이 현재 턴과 다를 경우
            {
                NetworkManager.Instance.WarningUIMake(text); // 경고 UI 생성
                return false;
            }

            return true;
        }

        // 비용 지불 가능 여부를 확인하는 함수
        private bool CanPayCost(int cost, string text)
        {
            if(!WalletEvent.OnCanUseGoldBar.Invoke(cost)) // 비용을 지불 할 수 없는 경우
            {
                NetworkManager.Instance.WarningUIMake(text); // 경고 UI 생성
                return false;
            }

            WalletEvent.OnUseGoldBar.Invoke(cost);
            return true;
        }

        // 자식 수를 통해서 남아있는 기물을 확인하는 함수
        private bool CheckLeftPieceCount(int leftPieceCount, string text)
        {
            if(leftPieceCount <= 0) // 자식 수가 0 이하라면
            {
                NetworkManager.Instance.WarningUIMake(text); // 경고 UI 생성
                return false;
            }

            return true;
        }

        // 누구 팀의 턴인지 확인하는 함수
        private bool CheckCurrentTurnTeam()
        {
            // 현재 턴의 팀과 내 팀이 다르다면
            if(TurnManager.Instance.CurrentTeamType != TeamManager.Instance.CurrentTeamType)
            {
                NetworkManager.Instance.WarningUIMake("현재 턴은 당신의 턴이 아닙니다.");
                return false;
            }

            return true;
        }
    }
}
// 마지막 작성 일자: 2025.09.04