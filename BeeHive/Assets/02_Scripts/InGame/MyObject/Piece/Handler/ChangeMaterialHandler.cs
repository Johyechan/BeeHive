using InGame.Helper;
using InGame.MyManager;
using InGame.MyObject.Piece.Data;
using System.Threading.Tasks;
using UnityEngine;

namespace InGame.MyObject.Piece.Handler
{
    // 작성자: 조혜찬
    // Material 변경 핸들러 클래스
    public class ChangeMaterialHandler
    {
        private Renderer _renderer; // 현재 객체의 랜더러

        private Material _originMaterial; // 기본 머티리얼
        private Material _emissionMaterial; // 특수 머티리얼

        private GameObject _currrentObj; // 현재 머티리얼을 변경하는 기물

        // 생성자(현재 객체의 랜더러, 기본 머티리얼, 특수 머티리얼을 가지는 구조체, 현재 머티리얼을 변경하는 기물 객체)
        public ChangeMaterialHandler(MaterialData materialData, GameObject currentObj)
        {
            _renderer = materialData.renderer;
            _originMaterial = materialData.originMaterial;
            _emissionMaterial = materialData.emissionMaterial;
            _currrentObj = currentObj;
        }

        public void ChangeMaterial(bool isChangeToOrigin)
        {
            if(isChangeToOrigin) // 기본 상태로 변경해야 한다면
            {
                _currrentObj.layer = LayerMask.NameToLayer("Default");
                ChangeMaterialHelper.ChangeMaterial(_renderer, _originMaterial); // 머티리얼 변경
            }
            else // 특수 상태로 변경해야 한다면
            {
                _currrentObj.layer = LayerMask.NameToLayer("ClickObj");
                ChangeMaterialHelper.ChangeMaterial(_renderer, _emissionMaterial); // 머티리얼 변경
            }
                
        }
    }
}
// 마지막 작성 일자: 2026.01.14