using InGame.MyEnum;
using InGame.MyManager.Local;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

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

        // 매개 변수로 받은 턴의 UI 애니메이션을 실행
        public async Task UIAnimationPlay(TurnType currentTurn)
        {
            InGameContext.Current.Data.PlacePlaneManager.FindCanPlacePlane();
            await _turnAnimations[currentTurn].UIAnimationPlay();
            _currentTurnTmpText.text = currentTurn.ToString();
        }
    }
}
// 마지막 작성 일자: 2026.02.06