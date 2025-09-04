using MyUtil.MyObjectPool;
using UnityEngine;

namespace InGame.MySystem
{
    // 작성자: 조혜찬
    // 지갑 관련(금화 및 금괴) 객체 생성 및 삭제 핸들러
    public class WalletObjectHandle
    {
        public void SetObject(Transform goldCoinParent, Transform goldBarParent, int goldCoinCount, int goldBarCount)
        {
            if (goldCoinParent.childCount < goldCoinCount) // 금화 객체가 실제 금화보다 적을 경우
            {
                MakeObject(goldCoinParent.childCount, goldCoinCount, ObjectPoolType.GoldCoin, goldCoinParent, 0.006f);
            }
            else if (goldCoinParent.childCount > goldCoinCount) // 금화 객체가 실제 금화보다 많을 경우
            {
                DestroyObject(goldCoinParent.childCount, goldCoinCount, ObjectPoolType.GoldCoin, goldCoinParent);
            }

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
                int index = childCount + i;
                GameObject obj = ObjectPoolManager.Instance.GetObject(type, parent); // 금화 또는 금괴 가져오기
                obj.transform.localPosition = new Vector3(index % 5 * interval, 0, index / 5 * -0.014f); // x축은 5개를 기준으로 interval * n 번째로 설정, z축은 5개마다 z축 - 0.014f 씩 내리기
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
// 마지막 작성 일자: 2025.08.26