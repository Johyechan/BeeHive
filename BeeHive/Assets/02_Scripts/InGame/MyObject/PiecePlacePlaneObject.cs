using System.Collections.Generic;
using UnityEngine;

namespace InGame.MyObject
{
    // �ۼ���: ������
    // �⹰ ��ġ ĭ�� ��� Ŭ����
    public class PiecePlacePlaneObject : PlacePlaneObjectBase
    {
        [SerializeField] private List<RoadPlacePlaneObject> _neerRoadPlaceTransformList; // ������ �پ��ִ� ���� ĭ�� �����ϴ� ����Ʈ
    }
}
// ������ �ۼ� ����: 2025.07.09