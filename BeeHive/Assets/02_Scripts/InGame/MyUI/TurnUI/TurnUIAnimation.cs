using InGame.MyEnum;
using InGame.MyUI.TurnUI;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace InGame.MyUI.TurnUI
{
    // 작성자: 조혜찬
    // 각 턴에 따라 실행될 UI 애니메이션을 가지는 클래스
    public class TurnUIAnimation : MonoBehaviour
    {
        [SerializeField] private Image _backgroundImage; // UI 애니메이션 백그라운드 이미지

        [SerializeField] private TMP_Text _tmpText; // 현재 턴을 보여주는 텍스트

        private Dictionary<TurnType, TurnUIAnimationHandlerBase> _turnAnimations = new Dictionary<TurnType, TurnUIAnimationHandlerBase>();

        // 변수 초기화
        private void Awake()
        {
            _turnAnimations.Add(TurnType.MakeTurn, new MakeTurnUIAnimationHandler(_backgroundImage, _tmpText));
            _turnAnimations.Add(TurnType.DrawTurn, new DrawTurnUIAnimationHandler(_backgroundImage, _tmpText));
            _turnAnimations.Add(TurnType.MainTurn, new MainTurnUIAnimationHandler(_backgroundImage, _tmpText));
            _turnAnimations.Add(TurnType.TurnEnd, new TurnEndUIAnimationHandler(_backgroundImage, _tmpText));
            _turnAnimations.Add(TurnType.ChangeTeam, new ChangeTeamUIAnimationHandler(_backgroundImage, _tmpText));
        }

        // 매개 변수로 받은 턴의 UI 애니메이션을 실행
        public void UIAnimationPlay(TurnType currentTurn)
        {
            _turnAnimations[currentTurn].UIAnimationPlay(); // UI 애니메이션 실행
        }
    }
}
// 마지막 작성 일자: 2025.07.31