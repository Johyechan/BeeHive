using DG.Tweening;
using InGame.MyEnum;
using InGame.MyManager;
using System.Threading.Tasks;
using UnityEngine;

namespace InGame.MyObject
{
    // 작성자: 조혜찬
    // 오브젝트 배열 관리 클래스의 부모 클래스
    public abstract class ObjectArrayBase : MonoBehaviour
    {
        [SerializeField] protected int _maxChild; // 최대 자식 수

        [SerializeField] private float _xPosPerChild; // x축 간격
        [SerializeField] private float _animationDelay; // 애니메이션 실행 시간
        [SerializeField] private float _angle; // 회전 각도

        // 자식 객체들 재배치 함수
        protected bool ObjectRePlace(Transform parent)
        {
            int objectCount = parent.childCount; // 현재 자식 수 - 즉 보유하고 있는 객체 수

            if (objectCount <= 0 || objectCount > _maxChild) // 보유 중인 객체 수가 0이하라면 또는 최대 보유 개수 초과라면
            {
                return false; // 반환
            }

            for(int i = 0; i < objectCount; i++)
            {
                float currentYPos = parent.GetChild(i).transform.position.y; // 현재 객체의 y축 위치를 저장 - x축과 z축을 전부 이동 후 y축을 움직이기 위함

                Transform trans = parent.GetChild(i); // 자식 객체의 Transform을 저장

                if(TurnManager.Instance.CurrentTeamType == TeamType.Team1) // 현재 턴이 Team1의 팀일 경우
                {
                    trans.transform.localRotation = Quaternion.Euler(0, 180, 0); // 카드를 180도 회전(회전을 안할 시 거꾸로 보임)
                }

                Sequence sequence = DOTween.Sequence() // 시퀀스를 통해 한 함수가 실행이 종료되고 다음 함수가 실행
                    .Append(trans.DOLocalMove(new Vector3(_xPosPerChild * i, currentYPos, 0), _animationDelay)) // 자식 객체를 x축과 z축은 옮겨야 할 위치로 이동
                    .Append(trans.DOLocalMoveY(0, _animationDelay)); // y축을 0으로 이동
            }
            return true;
        }
    }
}
// 마지막 작성 일자: 2025.11.25