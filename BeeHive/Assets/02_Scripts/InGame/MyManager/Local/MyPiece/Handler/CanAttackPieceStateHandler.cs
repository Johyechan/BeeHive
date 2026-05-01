using InGame.MyEnum;
using InGame.MyManager.Global;
using InGame.MyObject;
using InGame.MyObject.Piece;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tutorial;
using UnityEngine;

namespace InGame.MyManager.MyPiece.Handler
{
    // 작성자: 조혜찬
    // 공격 가능한 기물의 상태를 변화시키는 기능을 처리하는 핸들러
    public class CanAttackPieceStateHandler
    {
        // 공격 가능한 기물들을 보여주는 함수(공격하는 기물 타입, (Key)공격하는 객체의 타입에 따라 (Value)기물 리스트를 가지는 딕셔너리, 원거리 공격인지 여부 - 기본적으로 근접 공격)
        public void ShowCanAttackPieces(PieceBase attackingPiece, Dictionary<PieceBase, List<PieceBase>> canAttackPieceMap, Dictionary<PieceBase, List<PiecePlacePlaneObject>> canAttackPiecePlaceMap, bool isFirePowerAttack = false)
        {
            foreach (var piece in canAttackPieceMap) // 공격 가능 기물들 저장 맵 순회
            {
                if (piece.Key == attackingPiece) // 매개 변수로 받은 공격 하는 기물의 타입과 현재 순서의 타입이 같다면
                {
                    foreach (var pieceBase in piece.Value) // 해당 타입에 맞는 기물들을 저장한 리스트 순회
                    {
                        if(isFirePowerAttack) // 원거리 공격이라면
                        {
                            pieceBase.PieceVariable.isFirePowerAttackTarget = true; // 원거리 공격 대상으로 설정

                            foreach (var piecePlace in canAttackPiecePlaceMap)
                            {
                                if(attackingPiece == piecePlace.Key)
                                {
                                    foreach(var place in piecePlace.Value)
                                    {
                                        place.CanAttackHighLightOnOff(true);
                                    }
                                }
                            }
                        }

                        pieceBase.ChangeMaterial(false); // 머티리얼 변경
                    }
                    break;
                }
            }
        }

        // 공격 가능한 기물을 숨기는 함수((Key)객체의 타입에 따라 (Value)기물 리스트를 가지는 딕셔너리, 원거리 공격 여부 - 기본적으로 근접 공격)
        public void HideCanAttackPieces(Dictionary<PieceBase, List<PieceBase>> canAttackPieceMap, Dictionary<PieceBase, List<PiecePlacePlaneObject>> canAttackPiecePlaceMap, bool isFirePowerAttack = false, bool changeFirePowerAttack = false)
        {
            foreach (var piece in canAttackPieceMap) // 공격 가능 기물들 저장 맵 순회
            {
                foreach (var pieceBase in piece.Value) // 해당 타입에 맞는 기물들을 저장한 리스트 순회
                {
                    if (isFirePowerAttack) // 원거리 공격이라면
                    {
                        if (changeFirePowerAttack) // 원거리 공격 대상을 변경하는 상태라면
                        {
                            pieceBase.PieceVariable.isFirePowerAttackTarget = false; // 원거리 공격 대상에서 제외
                        }

                        foreach (var piecePlace in canAttackPiecePlaceMap)
                        {
                            foreach (var place in piecePlace.Value)
                            {
                                place.CanAttackHighLightOnOff(false);
                            }
                        }
                    }

                    pieceBase.ChangeMaterial(true);
                }
            }
        }
    }
}
// 마지막 작성 일자: 2026.04.30