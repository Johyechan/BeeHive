using InGame.MyManager.Global;
using InGame.MyObject.Interface;
using UnityEngine;

namespace InGame.MyManager.Local
{
    // 작성자: 조혜찬
    // 게임 맵 자식들을 관리하는 클래스
    public class GameMapManager : MonoBehaviour
    {
        // 네트워크 ID가 필요한 객체들에게 ID를 할당시켜주는 함수
        public void SetNetworkID()
        {
            var allChildren = transform.GetComponentsInChildren<INetworkIdObject>(true); // INetworkIdObject를 상속하는 모든 자식 탐색(비활성화 객체 포함)

            for (int i = 0; i < allChildren.Length; i++) // 네트워크 ID가 필요한 자식들 순회
            {
                var child = allChildren[i];
                child.NetworkId = ++ObjectIdManager.Instance.Id; // ID 증가(고유 ID 할당)
                ObjectIdManager.Instance.AddObject(child.NetworkId, child.CurrentObject); // 클라에도 ID 저장
            }
        }
    }
}
// 마지막 작성 일자: 2026.02.12