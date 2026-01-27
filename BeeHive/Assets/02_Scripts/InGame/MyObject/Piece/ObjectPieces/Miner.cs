using InGame.MyEnum;
using InGame.MyEvent;
using InGame.MyManager;
using InGame.MyManager.MyPiece;
using InGame.MyManager.MyPlacePlane;
using InGame.MyManager.Turn;
using InGame.MyObject.Piece.Data;
using System.Threading.Tasks;

namespace InGame.MyObject.Piece.ObjectPieces
{
    // 작성자: 조혜찬
    // 광부 기물 클래스
    public class Miner : PieceBase
    {
        protected override void Awake()
        {
            base.Awake();

            ParentSet();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            TurnEvents.OnMakeTurn.Add(Dig); // 생산 턴에 광부가 금화를 얻는 기능 큐에 추가
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            TurnEvents.OnMakeTurn.Remove(Dig); // 생산 턴에 광부가 금화를 얻는 기능 큐에서 삭제
        }

        // 금화를 얻는 함수
        private async Task Dig()
        {
            if (TurnManager.Instance.CurrentTeamType != CurrentTeamType) // 현재 턴 팀과 나의 팀이 다르다면
                return; // 반환

            if (_pieceVariable.currentPlacePlane == null) // 자기가 배치된 판이 없다면
                return; // 반환

            if (PieceManager.Instance.IsDrought) // 가뭄 상태라면
                return; // 반환

            if (!CanDig()) // 생산 불가한 상태라면
                return;

            switch (CurrentTeamType)
            {
                case TeamType.Team1: // 플레이어의 팀이 Team1일 경우
                    WalletEvent.OnGetGoldCoin?.Invoke(PieceVariable.currentPlacePlane.team1GoldCoin); // 현재 칸에서 team1이 얻는 금화만큼 얻기
                    break;
                case TeamType.Team2: // 플레이어의 팀이 Team2일 경우
                    WalletEvent.OnGetGoldCoin?.Invoke(PieceVariable.currentPlacePlane.team2GoldCoin); // 현재 칸에서 team2가 얻는 금화만큼 얻기
                    break;
                case TeamType.Team3: // 플레이어의 팀이 Team3일 경우
                    WalletEvent.OnGetGoldCoin?.Invoke(PieceVariable.currentPlacePlane.team3GoldCoin); // 현재 칸에서 team3이 얻는 금화만큼 얻기
                    break;
            }

            await Task.CompletedTask; // Taks 완료 반환
        }

        // 생산 가능 여부 확인 함수
        private bool CanDig()
        {
            PlacePlaneManager.Instance.Variable.findCanPlacePlaneSystem.FindCanMovePlacePlane(_pieceVariable.currentPlacePlane, CurrentTeamType, ObjectType.Miner);

            foreach (var plane in PlacePlaneManager.Instance.Variable.highLightHandler.CanDigCheckPlacePlanes) // 생산 가능 여부 확인 배치칸 순회
            {
                if (plane.isNearToCastle) // 성과 근접한 기물 배치칸이 있다면
                {
                    return true; 
                }
            }

            return false;
        }

        // 부모 초기화 함수
        private void ParentSet()
        {
            PieceVariable.parent = TeamManager.Instance.GetMinerTransform(TeamManager.Instance.CurrentTeamType); // 광부 객체의 부모 할당
        }
    }
}
// 마지막 작성 일자: 2026.01.27