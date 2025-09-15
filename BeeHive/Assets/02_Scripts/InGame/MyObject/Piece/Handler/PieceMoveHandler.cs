using DG.Tweening;
using InGame.MyManager;
using InGame.MyObject.Piece.Struct;
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

        public async Task MoveToPlacePlane(Transform parent, Vector3 targetPos, float angle = 0)
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

            // 시퀀스를 통해서 차례대로 순차적으로 실행
            await DOTween.Sequence()
                // 높이 먼저 올리기
                .Append(_pieceBase.transform.DOLocalMoveY(yPos, _pieceData.animationDuration))
                // 지정한 위치로 이동
                .Append(_pieceBase.transform.DOLocalMove(new Vector3(targetPos.x, yPos, targetPos.z), _pieceData.animationDuration))
                // 회전 값만큼 y축 회전
                .Append(_pieceBase.transform.DOLocalRotate(new Vector3(0, angle, 0), _pieceData.animationDuration))
                // 이후 높이 맞추기
                .Append(_pieceBase.transform.DOLocalMoveY(targetPos.y, _pieceData.animationDuration))
                // UI 상호작용 가능 상태로 초기화
                .AppendCallback(() => UIManager.Instance.CanInteractionUI = true)
                .AsyncWaitForCompletion();
        }
    }
}
// 마지막 작성 일자: 2025.09.15