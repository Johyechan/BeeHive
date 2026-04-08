using MyUtil;
using MyUtil.MyObjectPool;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace InGame.MyManager.Global
{
    // 작성자: 조혜찬
    // UI 관련 싱글톤 매니저 클래스
    public class UIManager : MonoSingleton<UIManager>
    {
        private bool _canInteractionUI; // UI 상호작용 가능 여부 변수
        public bool CanInteractionUI { get { return _canInteractionUI; } set { _canInteractionUI = value; } } // UI 상호작용 가능 여부 프로퍼티

        protected override void Awake()
        {
            base.Awake();
            _canInteractionUI = true; // 처음에는 UI 상호작용 가능하도록 초기화
            Ready();
        }

        public void WarningUIMake(string text)
        {
            GameObject canvas = GameObject.Find("Canvas"); // 캔버스 찾기
            GameObject uiPanel = ObjectPoolManager.Instance.GetObject(ObjectPoolType.UIPanel, canvas.transform); // 경고, 알림 UI 프리팹 가져오기
            CanvasGroup canvasGroup = uiPanel.GetComponent<CanvasGroup>();
            RectTransform rect = uiPanel.GetComponent<RectTransform>();
            canvasGroup.alpha = 1.0f; // 불투명도를 최대로 하여 보이도록 하기
            rect.anchoredPosition = Vector2.zero; // 위치 초기화
            TMP_Text tmpText = uiPanel.transform.GetChild(2).GetComponent<TMP_Text>(); // 텍스트 가져오기
            tmpText.text = text; // 경고, 알림 작성
        }
    }
}
// 마지막 작성 일자: 2026.04.08