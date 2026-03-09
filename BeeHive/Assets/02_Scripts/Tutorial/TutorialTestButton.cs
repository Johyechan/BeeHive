using InGame.MyManager;
using InGame.MyUI.MyUIInterface;
using UnityEngine;
using UnityEngine.SceneManagement;

// 튜토리얼 테스트를 위한 버튼 클래스 - 테스트 이후 삭제
public class TutorialTestButton : MonoBehaviour, IUIClick
{
    public void OnUIClick()
    {
        SceneManager.LoadScene(4);
        SceneMgr.Instance.IsTutorial = true;
    }
}
