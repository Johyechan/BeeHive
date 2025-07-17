using System;
using UnityEngine;

public static class HighLightEventSystem
{
    public static Action<bool> OnPieceHighLight; // 하이라이트를 키거나 끌 때 불릴 이벤트
    public static Action<bool> OnRoadHighLight; // 하이라이트를 키거나 끌 때 불릴 이벤트
}
