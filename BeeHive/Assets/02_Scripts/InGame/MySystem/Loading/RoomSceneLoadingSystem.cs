using DG.Tweening;
using UnityEngine;

namespace InGame.MySystem.Loading
{
    // 작성자: 조혜찬
    // 방 씬 로딩 관리 
    public class RoomSceneLoadingSystem : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _loadingUICanvasGroup; // 로딩 UI 창

        [SerializeField] private float _animationDuration; // 애니메이션 지속 시간

        private async void Awake()
        {
            await RoomReady.WaitAsync(); // 방 준비 대기

            await _loadingUICanvasGroup.DOFade(0, _animationDuration).AsyncWaitForCompletion(); // 로딩 창 닫기
            _loadingUICanvasGroup.gameObject.SetActive(false); // 비활성화
        }
    }
}
// 마지막 작성 일자: 2025.12.18