#if UNITY_EDITOR
using InGame.MyObject;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AutoFindNearObject : EditorWindow
{
    public Transform _parentTransform; // ��ġ ĭ���� ���� Ȯ���ϱ� ���� ��ġ ĭ �θ�

    public float _distance; // �� ���� �Ÿ����� �ִ� ���ε��� ã���� ���ϱ� ���� �Ÿ�

    private Dictionary<Transform, PiecePlacePlaneObject> _pieceMap = new(); // �⹰ ��ġ ĭ���� �����ϴ� ��
    private Dictionary<Transform, RoadPlacePlaneObject> _roadMap = new(); // ���� ��ġ ĭ���� �����ϴ� ��

    [MenuItem("Tools/Auto Find Near Object")] // �޴���
    public static void ShowWindow() // �޴����� ȣ�� ������ ���� �Լ�
    {
        GetWindow<AutoFindNearObject>("Auto Find Near Road"); // Unity �����Ϳ��� AutoFindNearRoadŸ���� Ŀ���� �����츦 ���� - Ÿ��Ʋ�� "Auto Find Near Road"
    }

    private void OnGUI()
    {
        _parentTransform = (Transform)EditorGUILayout.ObjectField("Place Plane Object Parent", _parentTransform, typeof(Transform), true); // ��ġ ĭ���� �θ� �ʵ�� Transform Ÿ������ �Ҵ�ޱ� - �̸�, ���� ���� ����, �� Ÿ��, �� ������Ʈ�� �ʵ忡 �Ҵ��� �� �ִ��� + �Ű������� ���� �����鼭 �� �ٽ� ������ �Ҵ����ִ� ������ ���� �ڵ忡�� ���� ���� ����ϱ� ���ؼ�

        _distance = EditorGUILayout.FloatField("Distance", _distance); // �󸶳� �������ִ� �Ÿ��� ��ü�� ������ ��ü�� �����ϰ� ������ ��ü ����Ʈ�� ������ �Ǵ��ϱ� ���� ������ float Ÿ������ �ʱ�ȭ

        if(GUILayout.Button("Find Near Object")) // ���� Find Near Object ��ư�� ���ȴٸ�
        {
            FindNearObject(); // �ڵ����� ������ ��ü�� ã�� �Լ� ����
        }
    }

    // Ÿ�Կ� �°� �ڵ����� ������ ��ü�� ã�� �Լ�
    public void FindNearObject()
    {
        _pieceMap.Clear(); // �� �ʱ�ȭ
        _roadMap.Clear(); // �� �ʱ�ȭ

        int placePlaneCount = _parentTransform.childCount; // �ڽ� ��(��ġ ĭ ��) ����

        for(int i = 0; i <  placePlaneCount; i++) // ��ġ ĭ ����ŭ �ݺ�
        {
            var child = _parentTransform.GetChild(i); // ���� �ڽ�(��ġĭ) Transform ����
            if(child.TryGetComponent<PiecePlacePlaneObject>(out PiecePlacePlaneObject piecePlacePlane)) // i��° �ڽ��� �⹰ ĭ���� Ȯ��
            {
                if (!_pieceMap.ContainsKey(child)) // ���� Transform�� Ű ���� ���� ���
                    _pieceMap.Add(child, piecePlacePlane); // �⹰ ĭ �ʿ� �߰� (�ش� ��ü�� Transform�� �ش� ��ü�� Ŭ����)
            }
            else if (child.TryGetComponent<RoadPlacePlaneObject>(out RoadPlacePlaneObject roadPlacePlane)) // i��° �ڽ��� ���� ĭ���� Ȯ��
            {
                if (!_roadMap.ContainsKey(child)) // ���� Transform�� Ű ���� ���� ���
                    _roadMap.Add(child, roadPlacePlane); // ���� ĭ �ʿ� �߰� (�ش� ��ü�� Transform�� �ش� ��ü�� Ŭ����)
            }
            else // ���� ���� ĭ�� �⹰ ĭ�� �ƴ϶��
            {
                Debug.Log($"{_parentTransform.name}�� {i}��° �ڽ�: �⹰ ��ġ ĭ, ���� ��ġ ĭ �� �� �� ���� �ƴ� �̻��� ��ü�� ������"); // ����׸� ���� ��ġĭ�� �ƴ϶�� �˸�
            }
        }

        foreach(var pieceData in _pieceMap)
        {
            PiecePlacePlaneObject piecePlacePlane = pieceData.Value; // �⹰ ĭ Ŭ���� ����
            piecePlacePlane.nearRoadPlaceTransformList.Clear(); // ����Ʈ �ʱ�ȭ
        }

        foreach(var roadData in _roadMap)
        {
            RoadPlacePlaneObject roadPlacePlane = roadData.Value; // ���� ĭ�� Ŭ���� ����
            roadPlacePlane.nearPiecePlaceTransformList.Clear(); // ����Ʈ �ʱ�ȭ
        }

        foreach(var pieceData in _pieceMap) // �⹰ ĭ �� ��ȸ
        {
            Vector3 piecePos = pieceData.Key.localPosition; // �⹰ ĭ�� ��ġ ����
            PiecePlacePlaneObject piecePlacePlane = pieceData.Value; // �⹰ ĭ Ŭ���� ����

            foreach(var roadData in _roadMap) // ���� ĭ �� ��ȸ
            {
                Vector3 roadPos = roadData.Key.localPosition; // ���� ĭ�� ��ġ ����
                RoadPlacePlaneObject roadPlacePlane = roadData.Value; // ���� ĭ�� Ŭ���� ����

                float distance = Vector3.Distance(piecePos, roadPos); // �⹰ ĭ�� ���� ĭ�� �Ÿ� ����

                if(distance <= _distance) // ���� �Ÿ��� ���� �Ÿ� ���϶��
                {
                    piecePlacePlane.nearRoadPlaceTransformList.Add(roadPlacePlane); // �⹰ ĭ Ŭ������ �ִ� ������ ���θ� �����ϴ� ����Ʈ�� ���� ���� Ŭ���� �߰�
                    roadPlacePlane.nearPiecePlaceTransformList.Add(piecePlacePlane); // ���� ĭ Ŭ������ �ִ� ������ �⹰�� �����ϴ� ����Ʈ�� ���� �⹰ Ŭ���� �߰�
                }
            }
        }

        Debug.Log("Auto Find Near Object End");
    }
}
#endif
// ������ �ۼ� ����: 2025.07.10