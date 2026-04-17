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
        private int _makeDelayMillisecond; // 생성 대기 시간

        private Color _originColor; // 기본 색

        private TMP_Text _otherTeamGoldCoin; // 상대 팀 골드 코인 개수 UI
        private TMP_Text _otherTeamGoldBar; // 상대 팀 골드 바 개수 UI

        private CancellationTokenSource _cts; // 작업 중지 토근

        public WalletObjectHandle(float team1GoldCoinInterval, float team1GoldBarInterval, float team2GoldCoinInterval, float team2GoldBarInterval, float zInterval, int zValueChangeCount, int goldBarMaxCount, int makeDelayMillisecond, Color originColor, TMP_Text otherTeamGoldCoin, TMP_Text otherTeamGoldBar)
        {
            _team1GoldCoinInterval = team1GoldCoinInterval;
            _team1GoldBarInterval = team1GoldBarInterval;
            _team2GoldCoinInterval = team2GoldCoinInterval;
            _team2GoldBarInterval = team2GoldBarInterval;
            _zInterval = zInterval;

            _zValueChangeCount = zValueChangeCount;
            _goldBarMaxCount = goldBarMaxCount;
            _makeDelayMillisecond = makeDelayMillisecond;

            _originColor = originColor;

            _otherTeamGoldCoin = otherTeamGoldCoin;
            _otherTeamGoldBar = otherTeamGoldBar;
        }

        public async Task SetObject(Transform goldCoinParent, Transform goldBarParent, int goldCoinCount, int goldBarCount, TeamType type)
        {
            _cts?.Cancel(); // 토큰이 존재한다면 정지
            _cts = new CancellationTokenSource(); // 새로운 토큰 할당
            var token = _cts.Token; // cts 토큰

            if(TeamManager.Instance.CurrentTeamType != type) // 내 팀의 금화 금괴 변경 사항이 아니라면
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

            int currentGoldCount = goldCoinParent.childCount; // 현재 금화 개수
            if (currentGoldCount < goldCoinCount) // 금화 객체가 실제 금화보다 적을 경우
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
                await MakeObject(currentGoldCount, goldCoinCount, ObjectPoolType.GoldCoin, goldCoinParent, interval, token);
            }
            else if (currentGoldCount > goldCoinCount) // 금화 객체가 실제 금화보다 많을 경우
            {
                await DestroyObject(currentGoldCount, goldCoinCount, ObjectPoolType.GoldCoin, goldCoinParent, token);
            }

            int currentGoldBarCount = goldBarParent.childCount;
            if (currentGoldBarCount < goldBarCount) // 금괴 객체가 실제 금괴보다 적을 경우
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
                await MakeObject(currentGoldBarCount, goldBarCount, ObjectPoolType.GoldBar, goldBarParent, interval, token);
            }
            else if (currentGoldBarCount > goldBarCount) // 금괴 객체 실제 금괴보다 많을 경우
            {
                await DestroyObject(currentGoldBarCount, goldBarCount, ObjectPoolType.GoldBar, goldBarParent, token);
            }
        }

        private async Task MakeObject(int childCount, int realCount, ObjectPoolType type, Transform parent, float interval, CancellationToken cts)
        {
            int count = realCount - childCount;
            if(type == ObjectPoolType.GoldCoin)
            {
                NetworkManager.Instance.Socket.Emit("debug", $"실제 개수: {realCount}, 자식 수: {childCount}, 더 생성해야 하는 개수: {count}");
            }
            for (int i = 0; i < count; i++) // 격차만큼 반복
            {
                if (cts.IsCancellationRequested) // 토큰에 취소 요청이 들어왔다면
                    return; // 반환

                int index = childCount + i;
                GameObject obj = ObjectPoolManager.Instance.GetObject(type, parent); // 금화 또는 금괴 가져오기
                obj.transform.localPosition = new Vector3(index % _zValueChangeCount * interval, ObjectPoolManager.Instance.AnimationYPos, index / _zValueChangeCount * _zInterval); // 금 개수가 z축 값이 변경되는 개수 초과이면 z축으로 _zInterval만큼 올라가고 x축은 초기화 돼서 0부터 다시 interval 간격으로 배치
                await ObjectPoolManager.Instance.Animation(obj, true, true, 0).AsyncWaitForCompletion(); // 애니메이션 실행 후 끝날 때까지 대기
                await Task.Delay(_makeDelayMillisecond);
            }
        }

        private async Task DestroyObject(int childCount, int realCount, ObjectPoolType type, Transform parent, CancellationToken cts)
        {
            for (int i = childCount - 1; i >= realCount; i--) // 끝부터 실제 개수까지 반복
            {
                if (cts.IsCancellationRequested) // 토큰에 취소 요청이 들어왔다면
                    return; // 반환

                GameObject obj = parent.GetChild(i).gameObject; // 금화 객체 저장
                ObjectPoolManager.Instance.ReturnObject(type, obj, true); // 금화 객체 오브젝트 풀에 다시 반환
                await Task.Delay(_makeDelayMillisecond);
            }
        }
    }
}
// 마지막 작성 일자: 2026.04.17