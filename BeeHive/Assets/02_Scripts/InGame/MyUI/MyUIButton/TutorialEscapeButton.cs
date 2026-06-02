using InGame.MyManager;
using InGame.MyManager.Enum;
using InGame.MyUI.MyUIInterface;
using UnityEngine;

namespace InGame.MyUI.MyUIButton
{
    // 작성자: 조혜찬
    // 튜토리얼 종료 버튼
    public class TutorialEscapeButton : MonoBehaviour, IUIClick
    {
        public void OnUIClick()
        {
            Time.timeScale = 1; // 시간 다시 흐르기
            SceneMgr.Instance.ChangeCurrentSceneFlow(SceneFlowType.GoLobby); // 로비 씬으로 이동하는 흐름으로 변경
            SceneMgr.Instance.LoadScene(); // 씬 전환
        }
    }
}
// 마지막 작성 일자: 2026.06.02