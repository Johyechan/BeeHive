using System;
using UnityEngine;

namespace InGame.MyManager.Team
{
    // 작성자: 조혜찬
    // 팀 매니저가 필요한 함수들을 실행 시키는 이벤트들을 가지는 클래스
    public static class TeamManagerEvents
    {
        public static Action OnNeedTeamManagerEvent; // 팀 매니저가 필요한 작업 액션
    }
}
// 마지막 작성 일자: 2026.01.19