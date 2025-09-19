using System;
using UnityEngine;

namespace InGame.MyObject.Piece.Data
{
    // 작성자: 조혜찬
    // Material과 관련된 변수들을 가지는 구조체
    [Serializable] // 직렬화를 통한 Inspector창에서도 변수 할당이 가능하도록 설정
    public struct MaterialData
    {
        public Renderer renderer; // 실시간으로 머티리얼을 변경하기 위한 랜더러

        public Material originMaterial; // 기본 머티리얼
        public Material emissionMaterial; // 발광 머티리얼
    }
}
// 마지막 작성 일자: 2025.09.15