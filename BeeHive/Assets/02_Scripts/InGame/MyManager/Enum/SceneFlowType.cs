using UnityEngine;

namespace InGame.MyManager.Enum
{
    // 작성자: 조혜찬
    // 씬 흐름 타입
    public enum SceneFlowType
    {
        None = 0,
        GoLobby, // 로비로 가는 흐름
        GoRoom, // 방으로 가는 흐름
        GoGame, // 게임으로 가는 흐름
        GoTutorial // 튜토리얼로 가는 흐름
    }
}
// 마지막 작성 일자: 2026.04.19