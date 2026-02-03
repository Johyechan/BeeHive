using InGame.MyEvent;
using InGame.MyManager;
using InGame.MyManager.Global;
using InGame.MySystem.Game;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace InGame.MySystem
{
    // 작성자: 조혜찬
    // 지갑 클래스
    public class Wallet : MonoBehaviour
    {
        [SerializeField] private TMP_Text _goldCoinTmpText; // 금화 개수 UI
        [SerializeField] private TMP_Text _goldBarTmpText; // 금괴 개수 UI

        private int _goldCoinCount = 0; // 금화 개수
        private int _goldBarCount = 0; // 금괴 개수

        private GoldSetHandle _goldSetHandle;

        private WalletUIHandle _walletUIHandle; // 지갑 UI 핸들러

        private WalletObjectHandle _walletObjectHandle; // 지갑 관련 객체 핸들러
        public WalletObjectHandle WalletObjectHandle { get => _walletObjectHandle; } // 위 변수 프로퍼티

        private void Awake()
        {
            _goldSetHandle = new GoldSetHandle(this);
            _walletUIHandle = new WalletUIHandle(_goldCoinTmpText, _goldBarTmpText);
            _walletObjectHandle = new WalletObjectHandle();

            _walletUIHandle.SetUI(_goldCoinCount, _goldBarCount); // 금화, 금괴 UI 초기화
        }

        private void OnEnable()
        {
            WalletEvent.OnGetGoldCoin += GetGoldCoin; // 금화 획득 이벤트에 금화 획득 함수 구독
            WalletEvent.OnGetGoldBar += GetGoldBar; // 금괴 획득 이벤트에 금괴 획득 함수 구독
            WalletEvent.OnUseGoldBar += UseGoldBar; // 금괴 사용 이벤트에 금괴 사용 함수 구독
            WalletEvent.OnCanUseGoldBar += CanUseGoldBar; // 금괴 사용 여부 확인 이벤트에 금괴 사용 여부 확인 함수 구독
        }

        private void OnDisable()
        {
            WalletEvent.OnGetGoldCoin -= GetGoldCoin; // 금화 획득 이벤트에 금화 획득 함수 구독 해제
            WalletEvent.OnGetGoldBar -= GetGoldBar; // 금괴 획득 이벤트에 금괴 획득 함수 구독 해제
            WalletEvent.OnUseGoldBar -= UseGoldBar; // 금괴 사용 이벤트에 금괴 사용 함수 구독 해제
            WalletEvent.OnCanUseGoldBar -= CanUseGoldBar; // 금괴 사용 여부 확인 이벤트에 금괴 사용 여부 확인 함수 구독
        }

        // 금화를 얻는 함수(얻는 값)
        private void GetGoldCoin(int value)
        {
            _goldCoinCount += value; // 금화 증가

            ChangeGoldCoinToGoldBar(); // 함수를 통해 금화를 금괴로 치환

            GoldSetEventEmit();
            _walletUIHandle.SetUI(_goldCoinCount, _goldBarCount); // UI 변경
            _goldSetHandle.Setting((int)TeamManager.Instance.CurrentTeamType, _goldCoinCount, _goldBarCount); // 객체 변경
        }

        // 금괴를 얻는 함수(얻는 값)
        private void GetGoldBar(int value)
        {
            _goldBarCount += value; // 금괴 증가

            GoldSetEventEmit();
            _walletUIHandle.SetUI(_goldCoinCount, _goldBarCount); // UI 변경
            _goldSetHandle.Setting((int)TeamManager.Instance.CurrentTeamType, _goldCoinCount, _goldBarCount); // 객체 변경
        }

        // 금괴 사용 함수
        private bool UseGoldBar(int value)
        {
            if (!CanUseGoldBar(value)) // 사용하려는 값보다 금괴 수가 적다면
                return false; // false 반환

            _goldBarCount -= value; // 금괴 감소

            GoldSetEventEmit();
            _walletUIHandle.SetUI(_goldCoinCount, _goldBarCount); // UI 변경
            _goldSetHandle.Setting((int)TeamManager.Instance.CurrentTeamType, _goldCoinCount, _goldBarCount); // 객체 변경
            return true;
        }

        // 금괴 사용 여부 판단 함수
        private bool CanUseGoldBar(int value)
        {
            if (_goldBarCount < value) // 사용하려는 값보다 금괴 수가 적다면
                return false; // false 반환

            return true; // true 반환
        }

        // 금화를 금괴로 바꾸는 함수
        private void ChangeGoldCoinToGoldBar()
        {
            if (_goldCoinCount >= 5) // 금화가 5개 이상이라면
            {
                _goldCoinCount -= 5; // 금화 5개 감소
                _goldBarCount++; // 금괴 1 증가
            }

            GoldSetEventEmit();
        }

        private void GoldSetEventEmit()
        {
            ChangeGoldInfo goldSetInfo = new ChangeGoldInfo()
            {
                roomID = SceneMgr.Instance.CurrentRoomID, // 현재 방 ID
                goldCoinCount = _goldCoinCount, // 금화 개수
                goldBarCount = _goldBarCount // 금괴 개수
            };

            string json = JsonUtility.ToJson(goldSetInfo);
            NetworkManager.Instance.Socket.Emit("debug", $"현재 골드 변경을 보내는 팀: {TeamManager.Instance.CurrentTeamType}, 스팀 아이디: {NetworkManager.Instance.CurrentClientName}");
            NetworkManager.Instance.Socket.Emit("changeGold", json);
        }
    }
}
// 마지막 작성 일자: 2026.02.03