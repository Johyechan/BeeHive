using InGame.MyEnum;
using InGame.MyManager.Global;
using InGame.MyManager.Local;
using MyUtil.MyObjectPool;
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

        private Color _team1OriginalColor; // 팀 1기본 색
        private Color _team2OriginalColor; // 팀 2기본 색

        private TMP_Text _team1GoldCoinText; // 팀 1 골드 코인 개수 UI
        private TMP_Text _team1GoldBarText; // 팀 1 골드 바 개수 UI
        private TMP_Text _team2GoldCoinText; // 팀 2 골드 코인 개수 UI
        private TMP_Text _team2GoldBarText; // 팀 2 골드 바 개수 UI

        public WalletObjectHandle(float team1GoldCoinInterval, float team1GoldBarInterval, float team2GoldCoinInterval, float team2GoldBarInterval, float zInterval, int zValueChangeCount, int goldBarMaxCount, Color team1OriginalColor, Color team2OriginalColor, TMP_Text team1GoldCoinText, TMP_Text team1GoldBarText, TMP_Text team2GoldCoinText, TMP_Text team2GoldBarText)
        {
            _team1GoldCoinInterval = team1GoldCoinInterval;
            _team1GoldBarInterval = team1GoldBarInterval;
            _team2GoldCoinInterval = team2GoldCoinInterval;
            _team2GoldBarInterval = team2GoldBarInterval;
            _zInterval = zInterval;

            _zValueChangeCount = zValueChangeCount;
            _goldBarMaxCount = goldBarMaxCount;

            _team1OriginalColor = team1OriginalColor;
            _team2OriginalColor = team2OriginalColor;

            _team1GoldCoinText = team1GoldCoinText;
            _team1GoldBarText = team1GoldBarText;
            _team2GoldCoinText = team2GoldCoinText;
            _team2GoldBarText = team2GoldBarText;
        }

        // 금화 세팅 함수
        public void SetGoldCoin(Transform goldCoinParent, int goldCoinCount, TeamType type)
        {
            if (TeamManager.Instance.CurrentTeamType != type) // 내 팀의 금화 금괴 변경 사항이 아니라면
            {
                switch (TeamManager.Instance.CurrentTeamType)
                {
                    case TeamType.Team1: // 자신의 팀이 팀 1 일 경우
                        _team2GoldCoinText.text = $"x {goldCoinCount}"; // 팀 2 금화 UI 변경
                        break;
                    case TeamType.Team2: // 자신의 팀이 팀 2 일 경우
                        _team1GoldCoinText.text = $"x {goldCoinCount}"; // 팀 1 금화 UI 변경
                        break;
                }
            }

            float coinInterval = (type == TeamType.Team1) ? _team1GoldCoinInterval : _team2GoldCoinInterval;
            SyncObject(goldCoinCount, ObjectPoolType.GoldCoin, goldCoinParent, coinInterval);
        }

        // 금괴 세팅 함수
        public void SetGoldBar(Transform goldBarParent, int goldBarCount, TeamType type)
        {
            if (TeamManager.Instance.CurrentTeamType != type) // 내 팀의 금화 금괴 변경 사항이 아니라면
            {
                switch (TeamManager.Instance.CurrentTeamType)
                {
                    case TeamType.Team1: // 자신의 팀이 팀 1 일 경우
                        SetGoldBarUI(_team2GoldBarText, goldBarCount, _team2OriginalColor); // 팀 2 UI 변경
                        break;
                    case TeamType.Team2: // 자신의 팀이 팀 2 일 경우
                        SetGoldBarUI(_team1GoldBarText, goldBarCount, _team1OriginalColor); // 팀 1 UI 변경
                        break;
                }
            }

            float barInterval = (type == TeamType.Team1) ? _team1GoldBarInterval : _team2GoldBarInterval;
            SyncObject(goldBarCount, ObjectPoolType.GoldBar, goldBarParent, barInterval);
        }

        private void SetGoldBarUI(TMP_Text otherTeamGoldBarText, int goldBarCount, Color otherTeamOriginalColor)
        {
            otherTeamGoldBarText.text = $"x {goldBarCount}"; // 상대 팀 금괴 개수 UI 변경
            if (goldBarCount >= _goldBarMaxCount) // 금괴 수가 최대 개수 이상이라면
            {
                otherTeamGoldBarText.color = Color.yellow; // 노란색으로 변경
            }
            else // 금괴 수가 최대 개수 미만이라면
            {
                otherTeamGoldBarText.color = otherTeamOriginalColor; // 기본 색상으로 변경
            }
        }

        private void SyncObject(int targetCount, ObjectPoolType type, Transform parent, float interval)
        {
            int diff = targetCount - parent.childCount; // 목표 수와 현재 자식 수의 개수 차이

            if (diff > 0) // 현재 자식 수보다 목표 수가 더 많을 경우
            {
                for(int i = 0; i < diff; i++)
                {
                    GameObject obj = ObjectPoolManager.Instance.GetObject(type, parent); // 금화 또는 금괴 가져오기
                    int currentIndex = parent.childCount - 1;
                    obj.transform.localPosition = new Vector3(currentIndex % _zValueChangeCount * interval, ObjectPoolManager.Instance.AnimationYPos, currentIndex / _zValueChangeCount * _zInterval); // 금 개수가 z축 값이 변경되는 개수 초과이면 z축으로 _zInterval만큼 올라가고 x축은 초기화 돼서 0부터 다시 interval 간격으로 배치
                }

                for (int i = 0; i < targetCount; i++)
                {
                    GameObject obj = parent.GetChild(i).gameObject;
                    ObjectPoolManager.Instance.Animation(obj, true, true); // 애니메이션 실행
                }
            }
            else if(diff < 0) // 현재 자식 수가 목표 수 보다 더 많을 경우
            {
                for(int i = 0; i < -diff; i++)
                {
                    int lastIndex = parent.childCount - 1; // 맨 끝 자식 인덱스 가져오기
                    GameObject obj = parent.GetChild(lastIndex).gameObject; // 맨 끝 자식 가져오기
                    ObjectPoolManager.Instance.ReturnObject(type, obj, true); // 맨 끝 자식 반환
                }
            }
        }
    }
}
// 마지막 작성 일자: 2026.05.20