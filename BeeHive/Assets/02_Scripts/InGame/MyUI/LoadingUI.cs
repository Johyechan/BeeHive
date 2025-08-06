using UnityEngine;

namespace InGame.MyUI
{
    // 작성자: 조혜찬
    // 로딩창 UI
    public class LoadingUI : MonoBehaviour
    {
        // 씬이 변경될 때 자동으로 로딩창을 비활성화 시키기 위한 코드
        private void OnDestroy()
        {
            gameObject.SetActive(false); // 로딩창 객체 비활성화
        }
    }
}
// 마지막 작성 일자: 2025.08.06