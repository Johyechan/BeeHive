using DG.Tweening;
using InGame.MyEnum;
using InGame.MyManager.Enum;
using InGame.MyManager.Global;
using InGame.MyObject.Piece.Data;
using System.Threading.Tasks;
using UnityEngine;

namespace InGame.MyObject.Piece.Handler
{
    // 작성자: 조혜찬
    // 기물 이동 핸들러 클래스
    public class PieceMoveHandler
    {
        private PieceBase _pieceBase; // 기물 클래스

        private PieceData _pieceData; // 기물이 가지는 불변 변수 구조체

        // 생성자(현재 기물 객체)
        public PieceMoveHandler(PieceBase pieceBase, PieceData pieceData)
        {
            _pieceBase = pieceBase;
            _pieceData = pieceData;
        }

        public async Task MoveToPlacePlane(Transform parent, Vector3 targetPos, bool isMove = false, float angle = 0)
        {
            if (parent.name == "PlacePos")
            {
                if (_pieceBase.CurrentTeamType == TeamManager.Instance.CurrentTeamType) // 현재 팀의 기물일 경우
                    _pieceBase.gameObject.layer = LayerMask.NameToLayer("ClickObj");
            }
            else
            {
                _pieceBase.gameObject.layer = LayerMask.NameToLayer("Default");
            }

            _pieceBase.transform.SetParent(parent); // 부모 변경

            float yPos = 0;
            if (targetPos.y > 0)
                yPos = targetPos.y * 1.5f; // 이후 배치할 때 애니메이션 효과를 위해 1.5배를 하여 조금 더 높이 올려준다
            else // 0일 경우 곱하는 것이 의미가 없기 때문에
                yPos = targetPos.y + 0.05f; // 이후 배치할 때 애니메이션 효과를 위해 1.5를 더하여 조금 더 높이 올려준다

            // 높이 먼저 올리기
            await _pieceBase.transform.DOLocalMoveY(yPos, _pieceData.animationDuration).AsyncWaitForCompletion();

            // 지정한 위치로 이동
            await _pieceBase.transform.DOLocalMove(new Vector3(targetPos.x, yPos, targetPos.z), _pieceData.animationDuration).AsyncWaitForCompletion();

            if(!isMove) // 이동이 아닌 배치일 때
            {
                

                if (_pieceBase.CurrentObjectType != ObjectType.Road) // 도로가 아닐 경우
                {
                    // 회전 애니메이션
                    await _pieceBase.transform.DOLocalRotate(new Vector3(0, 360, 0), _pieceData.animationDuration, RotateMode.FastBeyond360).AsyncWaitForCompletion(); // 회전 모드를 360초과 회전을 허용 시켜 한 바퀴 회전하도록 설정(안할 경우 DOTween은 최단 경로를 선택하고 유니티는 Quaternion 기준으로 회전, 그리고 Quaternion에서 360도 = 0도 그래서 가만히 있는 것 처럼 보인다)
                }
                else // 도로 일 때
                {
                    // 각도 조정
                    await _pieceBase.transform.DOLocalRotate(new Vector3(0, angle, 0), _pieceData.animationDuration).AsyncWaitForCompletion();
                }
            }

            // y축 조정
            await _pieceBase.transform.DOLocalMoveY(targetPos.y, _pieceData.animationDuration).AsyncWaitForCompletion();

            SoundManager.Instance.SFXPlay(SFXType.DropSound); // 기물 놓는 효과음 실행
            
            UIManager.Instance.CanInteractionUI = true;
        }
    }
}
// 마지막 작성 일자: 2026.02.09