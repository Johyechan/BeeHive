using InGame.MyEnum;
using InGame.MyManager;
using MyUtil.MyObjectPool;
using System;
using UnityEngine;

namespace InGame.MySystem
{
    // 작성자: 조혜찬
    // 지갑 관련(금화 및 금괴) 객체 생성 및 삭제 핸들러
    public class WalletObjectHandle
    {
        public void SetObject(Transform goldCoinParent, Transform goldBarParent, int goldCoinCount, int goldBarCount)
        {
            Debug.Log($"금화: 객체 수: {goldCoinParent.childCount}, 실제 수: {goldCoinCount}");
            if (goldCoinParent.childCount < goldCoinCount) // 금화 객체가 실제 금화보다 적을 경우
            {
                MakeObject(goldCoinParent.childCount, goldCoinCount, ObjectPoolType.GoldCoin, goldCoinParent, 0.006f);
            }
            else if (goldCoinParent.childCount > goldCoinCount) // 금화 객체가 실제 금화보다 많을 경우
            {
                DestroyObject(goldCoinParent.childCount, goldCoinCount, ObjectPoolType.GoldCoin, goldCoinParent);
            }

            Debug.Log($"금괴: 객체 수: {goldBarParent.childCount}, 실제 수: {goldBarCount}");
            if (goldBarParent.childCount < goldBarCount) // 금괴 객체가 실제 금괴보다 적을 경우
            {
                MakeObject(goldBarParent.childCount, goldBarCount, ObjectPoolType.GoldBar, goldBarParent, 0.006f);
            }
            else if (goldBarParent.childCount > goldBarCount) // 금괴 객체 실제 금괴보다 많을 경우
            {
                DestroyObject(goldBarParent.childCount, goldBarCount, ObjectPoolType.GoldBar, goldBarParent);
            }
        }

        private void MakeObject(int childCount, int realCount, ObjectPoolType type, Transform parent, float interval)
        {
            int count = realCount - childCount;
            for (int i = 0; i < count; i++) // 격차만큼 반복
            {
                GameObject obj = ObjectPoolManager.Instance.GetObject(type, parent); // 금화 가져오기
                if(parent.childCount <= 1) // 객체를 parent의 자식으로 생성 후 parent의 자식 수가 1일 경우(처음으로 생성된 경우)
                {
                    obj.transform.localPosition = Vector3.zero; // 0, 0, 0으로 초기화
                }
                else if(parent.childCount > 1) // 처음으로 생성되지 않은 경우
                {
                    obj.transform.localPosition = parent.GetChild(parent.childCount - 1).transform.position + new Vector3(interval, ((childCount + i) / 5) * -0.014f, 0); // 객체 위치를 마지막 객체의 x축 + 간격만큼 설정 + 5개마다 y축 - 0.014f 씩 내리기
                }
            }
        }

        private void DestroyObject(int childCount, int realCount, ObjectPoolType type, Transform parent)
        {
            for (int i = childCount - 1; i >= realCount; i--) // 끝부터 실제 개수까지 반복
            {
                GameObject obj = parent.GetChild(i).gameObject; // 금화 객체 저장
                ObjectPoolManager.Instance.ReturnObject(type, obj); // 금화 객체 오브젝트 풀에 다시 반환
            }
        }
    }
}
// 마지막 작성 일자: 2025.08.21