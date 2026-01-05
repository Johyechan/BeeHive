using DG.Tweening;
using InGame.MyManager;
using InGame.MyUI.MyUIInterface;
using MyUtil;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace InGame.MyUI.MyUIButton
{
    // 작성자: 조헤찬
    // 닉네임 생성 버튼
    public class NickNameCreateButton : MonoBehaviour, IUIClick
    {
        [SerializeField] private CanvasGroup _nickNameDuplicateCanvasGroup; // 닉네임 중복 경고 UI

        [SerializeField] private TMP_InputField _nickNameInputField; // 닉네임 작성 필드

        [SerializeField] private float _fadeDuration; // 페이드 인 지속 시간

        [SerializeField] private int _tutorialSceneNumber; // 튜토리얼 씬 번호

        private string _currentNickName; // 현재 닉네임

        private void Awake()
        {
            NetworkManager.Instance.Socket.On("isNickNameDuplicate", (value) => // 닉네임 중복 여부
            {
                bool isDuplicate = value.GetValue<bool>(); // 중복 여부 bool 값으로 저장
                if (isDuplicate) // 중복이라면
                {
                    MainThreadDispatcher.Enqueue(() =>
                    {
                        _nickNameDuplicateCanvasGroup.gameObject.SetActive(true); // 닉네임 중복 경고 UI 활성화
                        _nickNameDuplicateCanvasGroup.DOFade(1, _fadeDuration); // 닉네임 중복 경고 UI 페이드 인
                    });
                }
                else // 중복이 아닐경우
                {
                    NetworkManager.Instance.CurrentClientName = _currentNickName; // 현재 닉네임 할당
                    SceneManager.LoadScene(_tutorialSceneNumber); // 튜토리얼 씬으로 이동
                }
            });
        }

        // 클릭 시 실행될 함수
        public void OnUIClick()
        {
            _currentNickName = _nickNameInputField.text;
            var payload = new
            {
                nickName = _currentNickName // 닉네임 할당
            };

            string json = JsonConvert.SerializeObject(payload);
            NetworkManager.Instance.Socket.Emit("checkIsNickNameDuplicate", json); // 닉네임 중복 확인 이벤트 호출
        }
    }
}
// 마지막 작성 일자: 2026.01.05