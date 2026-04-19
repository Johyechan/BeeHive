using InGame.MyManager;
using InGame.MyManager.Enum;
using InGame.MyUI.MyUIInterface;
using UnityEngine;

// 튜토리얼 테스트를 위한 버튼 클래스 - 테스트 이후 삭제
public class TutorialTestButton : MonoBehaviour, IUIClick
{
    public void OnUIClick()
    {
        SceneMgr.Instance.ChangeCurrentSceneFlow(SceneFlowType.GoTutorial);// 튜토리얼 씬으로 이동하는 흐름으로 변경
        SceneMgr.Instance.LoadScene();
    }
}
