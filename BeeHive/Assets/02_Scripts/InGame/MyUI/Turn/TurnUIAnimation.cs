using InGame.MyEnum;
using InGame.MyManager.Local;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace InGame.MyUI.Turn
{
    // 작성자: 조혜찬
    // 각 턴에 따라 실행될 작업을 가지는 클래스
    public class TurnUIAnimation : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup; // UI 애니메이션 전체 페이드인, 아웃을 하기 위한 canvasGroup

        [SerializeField] private TMP_Text _tmpText; // 현재 턴을 보여주는 텍스트

        [SerializeField] private TMP_Text _currentTurnTmpText; // 현재 턴을 알려주는 텍스트

        [SerializeField] private float _animationDuration; // 애니메이션 시간

        [SerializeField] private Color _team1Color; // 팀 1 색
        [SerializeField] private Color _team2Color; // 팀 2 색

        private Dictionary<TurnType, TurnUIAnimationHandlerBase> _turnAnimations = new Dictionary<TurnType, TurnUIAnimationHandlerBase>();

        // 변수 초기화
        private void Awake()
        {
            _turnAnimations.Add(TurnType.MakeTurn, new MakeTurnUIAnimationHandler(_canvasGroup, _tmpText, _animationDuration));
            _turnAnimations.Add(TurnType.DrawTurn, new DrawTurnHandler(_canvasGroup, _tmpText, _animationDuration));
            _turnAnimations.Add(TurnType.MainTurn, new MainTurnHandler(_canvasGroup, _tmpText, _animationDuration));
            _turnAnimations.Add(TurnType.TurnEnd, new TurnEndUIAnimationHandler(_canvasGroup, _tmpText, _animationDuration));
            _turnAnimations.Add(TurnType.ChangeTeam, new ChangeTeamTurnUIAnimationHandler(_canvasGroup, _tmpText, _animationDuration));
        }

        // 현재 턴을 알려주는 UI 세팅
        public void SetCurrentTurnUI(TurnType currentTurn)
        {
            // 현재 팀 차례에 맞춰 텍스트 색 변경
            switch (InGameContext.Current.Data.TurnManager.CurrentTeamType)
            {
                case TeamType.Team1:
                    _currentTurnTmpText.color = _team1Color;
                    break;
                case TeamType.Team2:
                    _currentTurnTmpText.color = _team2Color;
                    break;
            }

            switch (currentTurn)
            {
                case TurnType.MakeTurn:
                    string makeTurn = LocalizationSettings.StringDatabase.GetLocalizedString(
                        "Game",
                        "Game_UI_MakeTurn"
                    );
                    _currentTurnTmpText.text = $"{makeTurn}";
                    break;
                case TurnType.DrawTurn:
                    string drawTurn = LocalizationSettings.StringDatabase.GetLocalizedString(
                        "Game",
                        "Game_UI_DrawTurn"
                    );
                    _currentTurnTmpText.text = $"{drawTurn}";
                    break;
                case TurnType.MainTurn:
                    string mainTurn = LocalizationSettings.StringDatabase.GetLocalizedString(
                        "Game",
                        "Game_UI_MainTurn"
                    );
                    _currentTurnTmpText.text = $"{mainTurn}";
                    break;
                case TurnType.TurnEnd:
                    string turnEnd = LocalizationSettings.StringDatabase.GetLocalizedString(
                        "Game",
                        "Game_UI_TurnEnd"
                    );
                    _currentTurnTmpText.text = $"{turnEnd}";
                    break;
                case TurnType.ChangeTeam:
                    string changeTeam = LocalizationSettings.StringDatabase.GetLocalizedString(
                        "Game",
                        "Game_UI_ChangeTeamTurn"
                    );
                    _currentTurnTmpText.text = $"{changeTeam}";
                    break;
            }
        }

        // 매개 변수로 받은 턴의 UI 애니메이션을 실행
        public async Task UIAnimationPlay(TurnType currentTurn)
        {
            InGameContext.Current.Data.PlacePlaneManager.FindCanPlacePlane();

            switch(InGameContext.Current.Data.TurnManager.CurrentTeamType) // 현재 턴의 팀에 따라
            {
                case TeamType.Team1:
                    _tmpText.color = Color.red; // Team1 색인 빨간색으로 텍스트 색 변경
                    break;
                case TeamType.Team2:
                    _tmpText.color = Color.blue; // Team2 색인 파란색으로 텍스트 색 변경
                    break;
            }

            SetCurrentTurnUI(currentTurn);
            await _turnAnimations[currentTurn].UIAnimationPlay();
        }
    }
}
// 마지막 작성 일자: 2026.05.14