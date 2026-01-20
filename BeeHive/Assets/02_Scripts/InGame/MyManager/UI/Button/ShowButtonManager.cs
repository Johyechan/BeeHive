using DG.Tweening;
using InGame.MyEnum;
using MyUtil;
using UnityEngine;

namespace InGame.MyManager.UI.Button
{
    // 작성자: 조혜찬
    // UI를 노출 시키는 버튼의 매니저
    public class ShowButtonManager : MonoSingleton<ShowButtonManager>
    {
        private ShowUIType _currentShowType = ShowUIType.None; // 현재 노출된 UI 타입

        private Sequence _currentSequence = null; // 현재 시퀀스

        // 시퀀스 플레이 함수
        public void PlaySequence(Sequence sequence)
        {
            _currentSequence?.Kill(); // 현재 시퀀스 강제 종료
            _currentSequence = sequence; // 받은 시퀀스를 현재 시퀀스로 할당
        }

        // 현재 노출된 UI 확인 함수
        public bool IsShowType(ShowUIType showUIType)
        {
            return _currentShowType == showUIType;
        }

        // 현재 노출된 UI 할당 함수
        public void SetShowType(ShowUIType showUIType)
        {
            _currentShowType = showUIType;
        }
    }
}
// 마지막 작성 일자: 2026.01.20