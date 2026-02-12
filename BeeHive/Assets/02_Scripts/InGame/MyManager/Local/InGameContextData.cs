using InGame.MyManager.Local.MyCard;
using InGame.MyManager.Local.MyPiece;
using InGame.MyManager.Local.MyPlacePlane;
using InGame.MyManager.Local.Turn;
using InGame.MyManager.Local.UI.Button;
using System;

namespace InGame.MyManager.Local
{
    // 작성자: 조혜찬
    // InGameContext 변수 모음 구조체
    [Serializable]
    public struct InGameContextData
    {
        public CameraManager CameraManager;

        public CardManager CardManager;

        public DeckManager DeckManager;

        public DrawManager DrawManager;

        public GameManager GameManager;

        public InputManager InputManager;

        public PieceManager PieceManager;

        public PlacePlaneManager PlacePlaneManager;

        public ShowButtonManager ShowButtonManager;

        public TurnManager TurnManager;

        public GameMapManager GameMapManager;
    }
}
// 마지막 작성 일자: 2026.02.12