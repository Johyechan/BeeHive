#if UNITY_EDITOR
using InGame.MyObject;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AutoFindNearObject : EditorWindow
{
    public Transform _parentTransform; // 배치 칸들을 전부 확인하기 위한 배치 칸 부모

    public float _distance; // 얼마 정도 거리내에 있는 도로들을 찾을지 정하기 위한 거리

    private Dictionary<Transform, PiecePlacePlaneObject> _pieceMap = new(); // 기물 배치 칸들을 저장하는 맵
    private Dictionary<Transform, RoadPlacePlaneObject> _roadMap = new(); // 도로 배치 칸들을 저장하는 맵

    [MenuItem("Tools/Auto Find Near Object")] // 메뉴명
    public static void ShowWindow() // 메뉴에서 호출 가능한 정적 함수
    {
        GetWindow<AutoFindNearObject>("Auto Find Near Road"); // Unity 에디터에서 AutoFindNearRoad타입의 커스텀 윈도우를 생성 - 타이틀은 "Auto Find Near Road"
    }

    private void OnGUI()
    {
        _parentTransform = (Transform)EditorGUILayout.ObjectField("Place Plane Object Parent", _parentTransform, typeof(Transform), true); // 배치 칸들의 부모를 필드로 Transform 타입으로 할당받기 - 이름, 값을 받을 변수, 값 타입, 씬 오브젝트를 필드에 할당할 수 있는지 + 매개변수로 값을 받으면서 또 다시 변수에 할당해주는 이유는 실제 코드에서 받은 값을 사용하기 위해서

        _distance = EditorGUILayout.FloatField("Distance", _distance); // 얼마나 떨어져있는 거리의 객체를 인접한 객체로 인지하고 인접한 객체 리스트에 넣을지 판단하기 위한 변수를 float 타입으로 초기화

        if(GUILayout.Button("Find Near Object")) // 만약 Find Near Object 버튼이 눌렸다면
        {
            FindNearObject(); // 자동으로 인접한 객체를 찾는 함수 실행
        }
    }

    // 타입에 맞게 자동으로 인접한 객체를 찾는 함수
    public void FindNearObject()
    {
        _pieceMap.Clear(); // 맵 초기화
        _roadMap.Clear(); // 맵 초기화

        int placePlaneCount = _parentTransform.childCount; // 자식 수(배치 칸 수) 저장

        for(int i = 0; i <  placePlaneCount; i++) // 배치 칸 수만큼 반복
        {
            var child = _parentTransform.GetChild(i); // 현재 자식(배치칸) Transform 저장
            if(child.TryGetComponent<PiecePlacePlaneObject>(out PiecePlacePlaneObject piecePlacePlane)) // i번째 자식이 기물 칸인지 확인
            {
                if (!_pieceMap.ContainsKey(child)) // 현재 Transform이 키 값에 없을 경우
                    _pieceMap.Add(child, piecePlacePlane); // 기물 칸 맵에 추가 (해당 객체의 Transform과 해당 객체의 클래스)
            }
            else if (child.TryGetComponent<RoadPlacePlaneObject>(out RoadPlacePlaneObject roadPlacePlane)) // i번째 자식이 도로 칸인지 확인
            {
                if (!_roadMap.ContainsKey(child)) // 현재 Transform이 키 값에 없을 경우
                    _roadMap.Add(child, roadPlacePlane); // 도로 칸 맵에 추가 (해당 객체의 Transform과 해당 객체의 클래스)
            }
            else // 만약 도로 칸도 기물 칸도 아니라면
            {
                Debug.Log($"{_parentTransform.name}의 {i}번째 자식: 기물 배치 칸, 도로 배치 칸 둘 중 한 개가 아닌 이상한 객체가 존재함"); // 디버그를 통해 배치칸이 아니라고 알림
            }
        }

        foreach(var pieceData in _pieceMap)
        {
            PiecePlacePlaneObject piecePlacePlane = pieceData.Value; // 기물 칸 클래스 저장
            piecePlacePlane.nearRoadPlaceTransformList.Clear(); // 리스트 초기화
        }

        foreach(var roadData in _roadMap)
        {
            RoadPlacePlaneObject roadPlacePlane = roadData.Value; // 도로 칸의 클래스 저장
            roadPlacePlane.nearPiecePlaceTransformList.Clear(); // 리스트 초기화
        }

        foreach(var pieceData in _pieceMap) // 기물 칸 맵 순회
        {
            Vector3 piecePos = pieceData.Key.localPosition; // 기물 칸의 위치 저장
            PiecePlacePlaneObject piecePlacePlane = pieceData.Value; // 기물 칸 클래스 저장

            foreach(var roadData in _roadMap) // 도로 칸 맵 순회
            {
                Vector3 roadPos = roadData.Key.localPosition; // 도로 칸의 위치 저장
                RoadPlacePlaneObject roadPlacePlane = roadData.Value; // 도로 칸의 클래스 저장

                float distance = Vector3.Distance(piecePos, roadPos); // 기물 칸과 도로 칸의 거리 측정

                if(distance <= _distance) // 만약 거리가 인접 거리 이하라면
                {
                    piecePlacePlane.nearRoadPlaceTransformList.Add(roadPlacePlane); // 기물 칸 클래스에 있는 인접한 도로를 저장하는 리스트에 현재 도로 클래스 추가
                    roadPlacePlane.nearPiecePlaceTransformList.Add(piecePlacePlane); // 도로 칸 클래스에 있는 인접한 기물을 저장하는 리스트에 현재 기물 클래스 추가
                }
            }
        }

        Debug.Log("Auto Find Near Object End");
    }
}
#endif
// 마지막 작성 일자: 2025.07.10