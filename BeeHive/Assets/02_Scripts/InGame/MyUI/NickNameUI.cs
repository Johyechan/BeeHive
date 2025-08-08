using InGame.MyManager;
using TMPro;
using UnityEngine;

namespace InGame.MyUI
{
    // 작성자: 조혜찬
    // 닉네임을 정하는 UI - 이후 스팀 닉네임으로 변경
    public class NickNameUI : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _nickNameField;

        [SerializeField] private TMP_Text _nickNameTmpText;

        public void InputEnd()
        {
            _nickNameTmpText.text = _nickNameField.text; // 플레이어가 정한 닉네임으로 변경
            NetworkManager.Instance.CurrentClientName = _nickNameTmpText.text; // 플레이어가 정한 닉네임 저장
            _nickNameField.text = "";
        }
    }
}
// 마지막 작성 일자: 2025.08.07