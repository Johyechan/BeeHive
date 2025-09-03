using InGame.MyManager;
using InGame.MyManager.MyCard;
using InGame.MyObject;
using UnityEngine;

namespace InGame.MySystem.Game
{
    // 작성자: 조혜찬
    // 상대 클라이언트의 변경 사항을 현재 클라이언트가 알 수 있도록 세팅하는 클래스
    public class GameSetting : MonoBehaviour
    {
        [SerializeField] private Wallet _wallet;

        private GoldSetHandle _goldSetEventHandle; // 금화 및 금괴 객체 세팅 핸들러

        private SetPieceHandle _setPieceHandle; // 기물 이동, 생성 핸들러

        private SetRoadHandle _setRoadHandle; // 도로 생성 핸들러

        private void Awake()
        {
            _goldSetEventHandle = new GoldSetHandle(_wallet); // 금화 및 금괴 객체 세팅 핸들러 생성
            _setPieceHandle = new SetPieceHandle(); // 기물 객체 이동 핸들러 생성
            _setRoadHandle = new SetRoadHandle(); // 도로 객체 생성 핸들러

            var socket = NetworkManager.Instance.Socket; // 서버와 통신하기 위한 객체 받아오기

            if(socket != null) // 서버와 통신하기 위한 객체가 존재할 경우
            {
                socket.On("goldSet", data =>
                {
                    string json = data.GetValue().ToString(); // 문자열로 data 받기
                    SetGoldInfo setGoldInfo = JsonUtility.FromJson<SetGoldInfo>(json); // SetGoldInfo 구조체로 값 받기
                    _goldSetEventHandle.Setting(setGoldInfo.team, setGoldInfo.goldCoin, setGoldInfo.goldBar); // 금화 및 금괴 객체 세팅(팀, 금화 개수, 금괴 개수)
                });

                socket.On("setCard", (data) =>
                {
                    string json = data.GetValue().ToString(); // 문자열로 data 받기
                    SetCardInfo setCardInfo = JsonUtility.FromJson<SetCardInfo>(json); // 카드 세팅에 필요한 값을 가지는 구조체로 값 받기
                    _ = DrawManager.Instance.CardSetHandle.Setting(setCardInfo.targetTeam, setCardInfo.cardCount); // Task 반환 없이 바로 실행
                });

                socket.On("setPiece", (data) =>
                {
                    string json = data.GetValue().ToString(); // 문자열로 data 받기
                    SetPieceInfo setPieceInfo = JsonUtility.FromJson<SetPieceInfo>(json); // 기물 세팅에 필요한 값을 가지는 구조체로 변경
                    _setPieceHandle.SetPiece(setPieceInfo.pieceID, setPieceInfo.placePlaneID, setPieceInfo.parentName, setPieceInfo.placedObjectType, setPieceInfo.targetPos, setPieceInfo.isMove); // 기물 세팅
                });

                socket.On("setRoad", (data) =>
                {
                    string json = data.GetValue().ToString(); // 문자열로 data 받기
                    SetRoadInfo setRoadInfo = JsonUtility.FromJson<SetRoadInfo>(json); // 도로 세팅에 필요한 값을 가지는 구조체로 변경
                    _setRoadHandle.SetRoad(setRoadInfo.placePlaneId, setRoadInfo.placedType, setRoadInfo.roadTeamType, setRoadInfo.roadParentName, setRoadInfo.targetParentName, setRoadInfo.targetPos, setRoadInfo.angle); // 도로 세팅
                });
            }
        }
    }
}
// 마지막 작성 일자: 2025.09.03