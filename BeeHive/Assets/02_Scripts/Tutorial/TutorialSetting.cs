using InGame;
using InGame.MyManager.Global;
using InGame.MyManager.Local;
using System.Collections.Generic;
using Tutorial.Struct;
using UnityEngine;

namespace Tutorial
{
    // 작성자: 조혜찬
    // 튜토리얼 세팅 클래스
    public class TutorialSetting : MonoBehaviour
    {
        [SerializeField] private List<TutorialRoadPlacePlaneData> _roadPlacePlanes;
        [SerializeField] private List<TutorialRoadData> _roads;
        [SerializeField] private List<TutorialPiecePlacePlaneData> _piecePlacePlanes;
        [SerializeField] private List<TutorialPieceData> _pieces;

        private async void Awake()
        {
            await GameReady.Gate.WaitAsync();

            Init();
        }

        // 초기화 함수
        private void Init()
        {
            NetworkManager.Instance.Socket.Emit("debug", "튜토리얼 초기화 함수 실행");

            foreach(var roadPlacePlane in  _roadPlacePlanes)
            {
                foreach(var road in _roads)
                {
                    
                    if(roadPlacePlane.connectNumber == road.connectNumber) // 연결 번호가 일치한다면
                    {
                        roadPlacePlane.roadPlacePlane.PlacedObjectType = road.road.CurrentObjectType; // 배치된 객체 타입 저장
                        roadPlacePlane.roadPlacePlane.TeamType = road.road.CurrentTeamType; // 배치된 객체 팀 저장
                        roadPlacePlane.roadPlacePlane.PlacedPiece = road.road; // 배치된 객체 저장

                        road.road.PieceVariable.currentRoadPlacePlane = roadPlacePlane.roadPlacePlane; // 배치 칸 저장
                    }
                }
            }

            foreach(var piecePlacePlane in _piecePlacePlanes)
            {
                foreach(var piece in _pieces)
                {
                    if (piecePlacePlane.connectNumber == piece.connectNumber) // 연결 번호가 일치한다면
                    {
                        piecePlacePlane.piecePlacePlane.PlacedObjectType = piece.piece.CurrentObjectType; // 배치된 객체 타입 저장
                        piecePlacePlane.piecePlacePlane.TeamType = piece.piece.CurrentTeamType; // 배치된 객체 팀 저장
                        piecePlacePlane.piecePlacePlane.PlacedPiece = piece.piece; // 배치된 객체 저장

                        piece.piece.PieceVariable.currentPlacePlane = piecePlacePlane.piecePlacePlane; // 배치 칸 저장
                    }
                }
            }

            InGameContext.Current.Data.PieceManager.FindCanPlacePlane();

            NetworkManager.Instance.Socket.Emit("debug", "튜토리얼 초기화 완료");
        }
    }
}
// 마지막 작성 일자: 2026.03.23