using InGame.MyEnum;
using InGame.MyManager;
using MyUtil.MyObjectPool;
using UnityEngine;

namespace InGame.MySystem
{
    // 작성자: 조혜찬
    // 지갑 관련(금화 및 금괴) 객체 생성 및 삭제 핸들러
    public class WalletObjectHandle
    {
        private Transform _goldCoinParent; // 금화 객체 부모
        private Transform _goldBarParent; // 금괴 객체 부모

        public void Init()
        {
            _goldCoinParent = GameObject.Find(TeamManager.Instance.GoldCoinParentName).transform;
            _goldBarParent = GameObject.Find(TeamManager.Instance.GoldBarParentName).transform;
        }

        public void SetObject(int goldCoinCount, int goldBarCount)
        {
            if(_goldCoinParent.childCount < goldCoinCount) // 금화 객체가 실제 금화보다 적을 경우
            {
                MakeObject(_goldCoinParent.childCount, goldCoinCount, ObjectPoolType.GoldCoin, _goldCoinParent, 0.006f);
            }
            else if(_goldCoinParent.childCount > goldCoinCount) // 금화 객체가 실제 금화보다 많을 경우
            {
                DestroyObject(_goldCoinParent.childCount, goldCoinCount, ObjectPoolType.GoldCoin, _goldCoinParent);
            }

            if (_goldBarParent.childCount < goldBarCount) // 금괴 객체가 실제 금괴보다 적을 경우
            {
                MakeObject(_goldBarParent.childCount, goldBarCount, ObjectPoolType.GoldBar, _goldBarParent, 0.006f);
            }
            else if (_goldBarParent.childCount > goldBarCount) // 금괴 객체 실제 금괴보다 많을 경우
            {
                DestroyObject(_goldBarParent.childCount, goldBarCount, ObjectPoolType.GoldBar, _goldBarParent);
            }
        }

        private void MakeObject(int childCount, int realCount, ObjectPoolType type, Transform parent, float interval)
        {
            int count = realCount - childCount;
            for (int i = 0; i < count; i++) // 격차만큼 반복
            {
                GameObject obj = ObjectPoolManager.Instance.GetObject(type, parent); // 금화 가져오기
                if(childCount <= 0)
                {
                    obj.transform.localPosition = Vector3.zero; // 0, 0, 0으로 초기화
                }
                else if(childCount > 0)
                {
                    // 여기 안오는 문제 있음
                    obj.transform.localPosition = parent.GetChild(childCount - 1).transform.localPosition + new Vector3(interval, ((childCount + i) / 5) * -0.014f, 0); // 객체 위치를 마지막 객체의 x축 + 간격만큼 설정 + 5개마다 y축 - 0.014f 씩 내리기
                }
                else
                {
                    obj.transform.localPosition = Vector3.zero; // 0, 0, 0으로 초기화
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
// 마지막 작성 일자: 2025.08.20