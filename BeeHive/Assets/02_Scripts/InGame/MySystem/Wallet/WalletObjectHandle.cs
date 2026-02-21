using InGame.MyEnum;
using MyUtil.MyObjectPool;
using UnityEngine;

namespace InGame.MySystem
{
    // 작성자: 조혜찬
    // 지갑 관련(금화 및 금괴) 객체 생성 및 삭제 핸들러
    public class WalletObjectHandle
    {
        private float _team1GoldCoinInterval;
        private float _team1GoldBarInterval;
        private float _team2GoldCoinInterval;
        private float _team2GoldBarInterval;

        public WalletObjectHandle(float team1GoldCoinInterval, float team1GoldBarInterval, float team2GoldCoinInterval, float team2GoldBarInterval)
        {
            _team1GoldCoinInterval = team1GoldCoinInterval;
            _team1GoldBarInterval = team1GoldBarInterval;
            _team2GoldCoinInterval = team2GoldCoinInterval;
            _team2GoldBarInterval = team2GoldBarInterval;
        }

        public void SetObject(Transform goldCoinParent, Transform goldBarParent, int goldCoinCount, int goldBarCount, TeamType type)
        {
            if (goldCoinParent.childCount < goldCoinCount) // 금화 객체가 실제 금화보다 적을 경우
            {
                float interval = 0;
                switch(type)
                {
                    case TeamType.Team1:
                        interval = _team1GoldCoinInterval;
                        break;
                    case TeamType.Team2:
                        interval = _team2GoldCoinInterval;
                        break;
                }
                MakeObject(goldCoinParent.childCount, goldCoinCount, ObjectPoolType.GoldCoin, goldCoinParent, interval);
            }
            else if (goldCoinParent.childCount > goldCoinCount) // 금화 객체가 실제 금화보다 많을 경우
            {
                DestroyObject(goldCoinParent.childCount, goldCoinCount, ObjectPoolType.GoldCoin, goldCoinParent);
            }

            if (goldBarParent.childCount < goldBarCount) // 금괴 객체가 실제 금괴보다 적을 경우
            {
                float interval = 0;
                switch (type)
                {
                    case TeamType.Team1:
                        interval = _team1GoldBarInterval;
                        break;
                    case TeamType.Team2:
                        interval = _team2GoldBarInterval;
                        break;
                }
                MakeObject(goldBarParent.childCount, goldBarCount, ObjectPoolType.GoldBar, goldBarParent, interval);
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
                obj.transform.localPosition = new Vector3(index * interval, 0, 0);
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
// 마지막 작성 일자: 2026.02.21