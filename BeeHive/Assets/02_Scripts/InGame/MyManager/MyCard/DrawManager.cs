using MyUtil;
using System;
using UnityEngine;

namespace InGame.MyManager.MyCard
{
    // �ۼ���: ������
    // ��ο츦 �����ϴ� �̱��� Ŭ����
    public class DrawManager : MonoSingleton<DrawManager>
    {
        public Func<bool> CanDraw; // ��ο찡 �������� Ȯ���ϴ� ��������Ʈ

        public bool IsCanDraw => CanDraw == null ? true : CanDraw.Invoke(); // ���� Func�� null�̶�� - ���� ��ο찡 �� ���� ������� ���� ����(�� ���� ī�尡 0���� ����)�̰� �׷��⿡ ��ο찡 ����ǵ� ������ ���⿡ true�� ��ȯ ���� null�� �ƴ� ������ ���� CanDraw�� ���� ��ȯ
    }
}
// ������ �ۼ� ����: 2025.07.08