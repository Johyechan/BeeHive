using System;

namespace InGame.MyUI
{
    // 작성자:
    // 가뭄을 사용할 때 필요한 값을 가지는 구조체
    [Serializable]
    public struct DroughtInfo
    {
        public string roomID; // 현재 방 ID
        public int targetTeam; // 가뭄을 사용한 대상 팀
        public int isDrought; // 가뭄 여부(1 = 참, 0 = 거짓)
    }
}
// 마지막 작성 일자: 2025.09.29