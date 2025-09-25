using MyUtil;
using UnityEngine;

namespace InGame.MyManager
{
    // 작성자: 조혜찬
    // 가지고 있는 카드들을 관리하는 매니저 싱글톤 클래스
    public class CardManager : MonoSingleton<CardManager>
    {
        private bool _haveFirePowerCard; // 화력 카드가 패에 있는지 여부
        public bool HaveFirePowerCard { get => _haveFirePowerCard; set => _haveFirePowerCard = value; } // 위 변수 프로퍼티
    }
}
// 마지막 작성 일자: 2025.09.25