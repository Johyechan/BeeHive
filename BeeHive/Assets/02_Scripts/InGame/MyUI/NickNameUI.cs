using InGame.MyManager;
using TMPro;
using UnityEngine;

namespace InGame.MyUI
{
    // 작성자: 조혜찬
    // 닉네임 UI
    public class NickNameUI : MonoBehaviour
    {
        private TMP_Text _nickName;

        private void Awake()
        {
            _nickName = GetComponent<TMP_Text>();
        }

        private void Start()
        {
            _nickName.text = NetworkManager.Instance.CurrentClientName;
        }
    }
}
// 마지막 작성 일자: 2026.01.07