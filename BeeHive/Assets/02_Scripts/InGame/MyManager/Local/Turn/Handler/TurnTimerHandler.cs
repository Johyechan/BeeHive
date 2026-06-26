using DG.Tweening;
using InGame.MyManager.Global;
using InGame.MyManager.Local;
using MyUtil.GameMode;
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace InGame.MyManager.Turn.Handler
{
    // 작성자: 조혜찬
    // 턴이 자동으로 넘어가는 시간을 재는 타이머 클래스
    public class TurnTimerHandler
    {
        private CancellationTokenSource _cts; // 취소 스위치

        // 턴 타이머 시작
        public void TurnTimerStart(Slider timerSlider, int time)
        {
            if(_cts != null) // 취소 스위치가 존재할 경우
            {
                _cts.Cancel(); // 취소 신호 발생
                _cts.Dispose(); // 내부 리소스 해제
                _cts = null; // null로 객체 비우기
            }

            _cts = new CancellationTokenSource(); // 새 취소 스위치 할당
            _ = Timer(timerSlider, time, _cts.Token); // 타이머 실행
        }

        public void TurnTimerStopImmediately()
        {
            _cts?.Cancel(); // 취소 스위치가 존재할 경우 취소 신호 발생
        }

        // 턴 타이머
        private async Task Timer(Slider timerSlider, int time, CancellationToken token)
        {
            try
            {
                if(GameModeManager.Instance.CurrentGameMode.UseServer())
                    NetworkManager.Instance.Socket.Emit("turnTimerStart", SceneMgr.Instance.CurrentRoomID);

                await Task.Delay(time * 1000, token); // time초 만큼 대기 또는 token 취소 발생 시 대기 종료
            }
            finally
            {
                TurnTimerEnd(timerSlider); // 턴 종료
            }
        }

        // 턴 종료 함수
        private void TurnTimerEnd(Slider timerSlider)
        {
            if (InGameContext.Current.Data.TurnManager.CurrentTeamType == TeamManager.Instance.CurrentTeamType) // 현재 턴의 팀이 내 팀일 경우
            {
                TurnCompletedInfo turnCompletedInfo = new TurnCompletedInfo()
                {
                    roomID = SceneMgr.Instance.CurrentRoomID, // 현재 방 ID
                    completedTurn = (int)InGameContext.Current.Data.TurnManager.CurrentTurnType // 현재 완료한 턴
                };
                string json = JsonUtility.ToJson(turnCompletedInfo); // Json으로 변환

                if(GameModeManager.Instance.CurrentGameMode.UseServer())
                    NetworkManager.Instance.Socket.Emit("turnCompleted", json); // 서버에 턴 변경 이벤트 전달

                InGameContext.Current.Data.TurnManager.CanChangeTurn = false; // 턴 변경 가능 여부 false로 초기화
            }
        }
    }
}
// 마지막 작성 일자: 2026.06.26