using System;
using System.Collections.Generic;

namespace MyUtil
{
    // 작성자: 조혜찬
    // 셔플 기능 유틸리티 클래스
    public static class ShuffleUtility
    {
        private static Random _random = new Random(); // 랜덤 함수

        // T 타입의 리스트를 받아 셔플을 시켜주는 함수(셔플 시킬 리스트)
        public static void Shuffle<T> (List<T> list)
        {
            T temp; // 임시 저장소
            for (int j = 0; j < list.Count; j++) // 현재 리스트 안에 있는 모든 객체를 순회
            {
                temp = list[j]; // 임시 저장소에 현재 값을 저장
                int random = _random.Next(j, list.Count); // j부터 리스트의 크기 - 1 수를 랜덤하게 반환
                list[j] = list[random]; // j번째 인덱스 값에 random 인덱스 값을 할당
                list[random] = temp; // 임시 저장소에 저장한 값을 random한 인덱스에 할당
            }
        }
    }
}
// 마지막 작성 일자: 2025.07.31