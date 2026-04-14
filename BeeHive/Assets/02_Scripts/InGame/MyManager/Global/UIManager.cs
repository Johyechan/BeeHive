using MyUtil;
using MyUtil.MyObjectPool;
using System.Collections.Generic;
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

        private const string RESOLUTION_KEY = "Resolution"; // PlayerPrefs 키

        public int CurrentResolutionIndex // 현재 해상도로 선택된 드롭다운 인덱스
        {
            get
            {
                return PlayerPrefs.GetInt(RESOLUTION_KEY, 0);
            }

            set
            {
                PlayerPrefs.SetInt(RESOLUTION_KEY, value);
            }
        }

        private Dictionary<int, Resolution> _resolutionMap = new Dictionary<int, Resolution>(); // 해상도 맵
        public Dictionary<int, Resolution> ResolutionMap { get => _resolutionMap; } // 해상도 맵 프로퍼티

        protected override void Awake()
        {
            base.Awake();
            _canInteractionUI = true; // 처음에는 UI 상호작용 가능하도록 초기화

            _resolutionMap.Add(0, CreateResolution(1920, 1080));
            _resolutionMap.Add(1, CreateResolution(1600, 900));
            _resolutionMap.Add(2, CreateResolution(1366, 768));
            _resolutionMap.Add(3, CreateResolution(1280, 720));
            _resolutionMap.Add(4, CreateResolution(2560, 1440));

            Ready();
        }

        // 해상도 생성 함수
        private Resolution CreateResolution(int width, int height)
        {
            Resolution resolution = new Resolution();
            resolution.width = width;
            resolution.height = height;
            return resolution;
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
// 마지막 작성 일자: 2026.04.14