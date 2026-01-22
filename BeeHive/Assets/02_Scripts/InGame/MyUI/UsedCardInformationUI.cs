using DG.Tweening;
using InGame.MyManager;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace InGame.MyUI
{
    // 작성자: 조혜찬
    // 사용된 카드를 알려주는 UI
    public class UsedCardInformationUI : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;

        [SerializeField] private TMP_Text _usedCardName; // 사용한 카드의 이름
        [SerializeField] private TMP_Text _usedCardInformation; // 사용한 카드의 정보(효과)

        [SerializeField] private float _animationDuration; // 애니메이션 지속시간
        [SerializeField] private float _usedCardUIShowSecond; // 사용한 카드를 보여주는 시간

        private float _makeMillisecondValue = 1000f; // 밀리세컨드로 변경시키는 변수


        private void Awake()
        {
            var socket = NetworkManager.Instance.Socket; // 서버와 통신할 소켓 할당
            if(socket != null) // 서버와 통신할 소켓이 있다면
            {
                socket.On("usedCardInformation", async (data) =>
                {
                    if (NetworkManager.Instance.IsClientOver) // 클라이언트가 종료 되었다면
                        return; // 반환

                    string json = data.GetValue().ToString(); // 문자열 형태로 값 받기
                    UsedCardInfo usedCardInfo = JsonUtility.FromJson<UsedCardInfo>(json); // UsedCardInfo 구조체 형태로 json 변환
                    _usedCardName.text = usedCardInfo.usedCardName; // 사용된 카드의 이름을 UI에 할당
                    _usedCardInformation.text = usedCardInfo.usedCardInformation; // 사용된 카드의 정보(효과)를 UI에 할당

                    await _canvasGroup.DOFade(1, _animationDuration).AsyncWaitForCompletion(); // 애니메이션 지속시간 동안 페이드 인

                    await Task.Delay((int)(_usedCardUIShowSecond * _makeMillisecondValue)); // 사용된 카드를 보여주는 시간만큼 대기

                    await _canvasGroup.DOFade(0, _animationDuration).AsyncWaitForCompletion(); // 애니메이션 지속시간 동안 페이드 아웃
                });
            }
        }
    }
}
// 마지막 작성 일자: 2026.01.22