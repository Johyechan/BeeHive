using MyUtil;
using System;
using UnityEngine;

namespace InGame.MyManager.MyCard
{
    // 작성자: 조혜찬
    // 드로우를 관리하는 싱글톤 클래스
    public class DrawManager : MonoSingleton<DrawManager>
    {
        public Func<bool> CanDraw; // 드로우가 가능한지 확인하는 델리게이트

        public bool IsCanDraw => CanDraw == null ? true : CanDraw.Invoke(); // 만약 Func가 null이라면 - 아직 드로우가 한 번도 진행되지 않은 상태(즉 보유 카드가 0개인 상태)이고 그렇기에 드로우가 진행되도 무리가 없기에 true를 반환 이후 null이 아닌 상태일 때는 CanDraw의 값을 반환
    }
}
// 마지막 작성 일자: 2025.07.08