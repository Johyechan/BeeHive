using DG.Tweening;
using InGame.MyEnum;
using InGame.MyManager.Global;
using MyUtil.MyObjectPool;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace InGame.MySystem
{
    // 작성자: 조혜찬
    // 지갑 관련(금화 및 금괴) 객체 생성 및 삭제 핸들러
    public class WalletObjectHandle
    {
        private float _team1GoldCoinInterval; // 팀1 금화 간격
        private float _team1GoldBarInterval; // 팀1 금괴 간격
        private float _team2GoldCoinInterval; // 팀2 금화 간격
        private float _team2GoldBarInterval; // 팀2 금괴 간격
        private float _zInterval; // z축 간격

        private int _zValueChangeCount; // z축 값이 변경되는 개수 
        private int _goldBarMaxCount; // 금괴 최대 개수

        private Color _originColor; // 기본 색

        private TMP_Text _otherTeamGoldCoin; // 상대 팀 골드 코인 개수 UI
        private TMP_Text _otherTeamGoldBar; // 상대 팀 골드 바 개수 UI

        public WalletObjectHandle(float team1GoldCoinInterval, float team1GoldBarInterval, float team2GoldCoinInterval, float team2GoldBarInterval, float zInterval, int zValueChangeCount, int goldBarMaxCount, Color originColor, TMP_Text otherTeamGoldCoin, TMP_Text otherTeamGoldBar)
        {
            _team1GoldCoinInterval = team1GoldCoinInterval;
            _team1GoldBarInterval = team1GoldBarInterval;
            _team2GoldCoinInterval = team2GoldCoinInterval;
            _team2GoldBarInterval = team2GoldBarInterval;
            _zInterval = zInterval;

            _zValueChangeCount = zValueChangeCount;
            _goldBarMaxCount = goldBarMaxCount;

            _originColor = originColor;

            _otherTeamGoldCoin = otherTeamGoldCoin;
            _otherTeamGoldBar = otherTeamGoldBar;
        }

        public void SetObject(Transform goldCoinParent, Transform goldBarParent, int goldCoinCount, int goldBarCount, TeamType type)
        {
            if (TeamManager.Instance.CurrentTeamType != type) // 내 팀의 금화 금괴 변경 사항이 아니라면
            {
                _otherTeamGoldCoin.text = $"x {goldCoinCount}"; // 상대 팀 금화 개수 UI 변경
                _otherTeamGoldBar.text = $"x {goldBarCount}"; // 상대 팀 금괴 개수 UI 변경
                if (goldBarCount >= _goldBarMaxCount) // 금괴 수가 최대 개수 이상이라면
                {
                    _otherTeamGoldBar.color = Color.red; // 빨간색으로 변경
                }
                else // 금괴 수가 최대 개수 미만이라면
                {
                    _otherTeamGoldBar.color = _originColor; // 기본 색상으로 변경
                }
            }

            float coinInterval = (type == TeamType.Team1) ? _team1GoldCoinInterval : _team2GoldCoinInterval;
            SyncObject(goldCoinCount, ObjectPoolType.GoldCoin, goldCoinParent, coinInterval);

            float barInterval = (type == TeamType.Team1) ? _team1GoldBarInterval : _team2GoldBarInterval;
            SyncObject(goldBarCount, ObjectPoolType.GoldBar, goldBarParent, barInterval);
        }

        private void SyncObject(int targetCount, ObjectPoolType type, Transform parent, float interval)
        {
            int currentCount = parent.childCount; // 현재 자식 수

            for(int i = currentCount; i < targetCount; i++)
            {
                GameObject obj = ObjectPoolManager.Instance.GetObject(type, parent); // 금화 또는 금괴 가져오기
                obj.transform.localPosition = new Vector3(i % _zValueChangeCount * interval, ObjectPoolManager.Instance.AnimationYPos, i / _zValueChangeCount * _zInterval); // 금 개수가 z축 값이 변경되는 개수 초과이면 z축으로 _zInterval만큼 올라가고 x축은 초기화 돼서 0부터 다시 interval 간격으로 배치
                obj.transform.DOKill(true);
                ObjectPoolManager.Instance.Animation(obj, true, true, 0); // 애니메이션 실행 후 끝날 때까지 대기
            }

            for(int i = currentCount - 1; i >= targetCount; i--)
            {
                GameObject obj = parent.GetChild(i).gameObject;
                ObjectPoolManager.Instance.ReturnObject(type, obj, true);
            }
        }
    }
}
// 마지막 작성 일자: 2026.04.20