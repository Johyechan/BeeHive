using InGame.MyManager;
using InGame.MyManager.Global;
using InGame.MyObject;
using MyUtil;
using System;
using System.Collections.Generic;

namespace InGame.MySystem.Game
{
    // 작성자: 조혜찬
    // 배치 가능한 판들에 하이라이트를 관리하기 위한 클래스
    public class HighLightHandler
    {
        private HashSet<PlacePlaneObjectBase> _canPiecePlacePlanes = new(); // 배치 가능한 기물 배치 판들을 저장해두는 해시 테이블 기반 컨테이너
        public HashSet<PlacePlaneObjectBase> CanPiecePlacePlanes { get { return _canPiecePlacePlanes; } } // _canPiecePlacePlanes 프로퍼티

        private HashSet<PlacePlaneObjectBase> _canRoadPlacePlanes = new(); // 배치 가능한 기물 배치 판들을 저장해두는 해시 테이블 기반 컨테이너
        public HashSet<PlacePlaneObjectBase> CanRoadPlacePlanes { get { return _canRoadPlacePlanes; } } // _canPlacePlanes 프로퍼티

        private HashSet<PlacePlaneObjectBase> _canPieceMovePlanes = new(); // 이동 가능한 기물 배치 판들을 저장해두는 해시 테이블 기반 컨테이너
        public HashSet<PlacePlaneObjectBase> CanPieceMovePlanes { get { return _canPieceMovePlanes; } } // _canMovePlacePlanes 프로퍼티

        private HashSet<PlacePlaneObjectBase> _canDigCheckPlacePlanes = new(); // 광부가 생산 가능한지 확인할 때 필요한 배치 칸들을 저장하는 해시 테이블 기반 컨테이너
        public HashSet<PlacePlaneObjectBase> CanDigCheckPlacePlanes { get { return _canDigCheckPlacePlanes; } } // 광부가 생산 가능한지 확인할 때 필요한 배치 칸들을 저장하는 해시 테이블 컨테이너 프로퍼티

        public void PieceHighLight(bool on, bool isPlace)
        {
            if (isPlace)
            {
                if (_canPiecePlacePlanes.Count <= 0) // 배치 가능한 기물 판 객체 존재하지 않다면
                {
                    return; // 그냥 반환
                }

                foreach (var placePlane in _canPiecePlacePlanes) // 배치 가능한 기물 판 객체들 순회
                {
                    if (on) // 킬 것이라면
                    {
                        NetworkManager.Instance.Socket.Emit("debug", $"켜지는 발판: {placePlane}");
                        placePlane.HighLightOn(); // 하이라이트 키기
                    }
                    else // 끌 것이라면
                    {
                        NetworkManager.Instance.Socket.Emit("debug", $"꺼지는 발판: {placePlane}");
                        placePlane.HighLightOff(); // 하이라이트 끄기
                    }
                }
            }
            else
            {
                if (_canPieceMovePlanes.Count <= 0) // 이동 가능한 기물 판 객체 존재하지 않다면
                {
                    return; // 그냥 반환
                }

                foreach (var placePlane in _canPieceMovePlanes) // 이동 가능한 기물 판 객체들 순회
                {
                    if (on) // 킬 것이라면
                    {
                        placePlane.HighLightOn(); // 하이라이트 키기
                    }
                    else // 끌 것이라면
                    {
                        placePlane.HighLightOff(); // 하이라이트 끄기
                    }
                }
            }
        }

        public void RoadHighLight(bool on)
        {
            if (_canRoadPlacePlanes.Count <= 0) // 배치 가능한 도로 판 객체 존재하지 않다면
            {
                return; // 그냥 반환
            }

            foreach (var placePlane in _canRoadPlacePlanes) // 배치 가능한 도로 판 객체들 순회
            {
                if (on) // 킬 것이라면
                {
                    placePlane.HighLightOn(); // 하이라이트 키기
                }
                else // 끌 것이라면
                {
                    placePlane.HighLightOff(); // 하이라이트 끄기
                }
            }
        }
    }
}
// 마지막 작성 일자: 2026.02.03