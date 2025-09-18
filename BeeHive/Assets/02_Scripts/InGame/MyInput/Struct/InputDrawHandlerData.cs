using UnityEngine;

namespace InGame.MyInput.Struct
{
    // 작성자: 조혜찬
    // InputDrawHandler에 필요한 핸들러들을 가지는 구조체
    public struct InputDrawHandlerData
    {
        public InputDrawReturnHandler returnHandler; // 반환 핸들러

        public InputDrawSocketEventHandler socketEventHandler; // 소켓 이벤트 핸들러

        public InputDrawFunctionHandler functionHandler; // 기능 핸들러
    }
}
