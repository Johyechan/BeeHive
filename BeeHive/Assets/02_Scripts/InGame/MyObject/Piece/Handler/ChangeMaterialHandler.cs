using InGame.Helper;
using InGame.MyObject.Piece.Struct;
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

        // 생성자(현재 객체의 랜더러, 기본 머티리얼, 특수 머티리얼
        public ChangeMaterialHandler(MaterialData materialData)
        {
            _renderer = materialData.renderer;
            _originMaterial = materialData.originMaterial;
            _emissionMaterial = materialData.emissionMaterial;
        }

        public async Task ChangeMaterial(bool isChangeToOrigin)
        {
            if(isChangeToOrigin) // 기본 상태로 변경해야 한다면
                await ChangeMaterialHelper.ChangeMaterial(_renderer, _originMaterial); // 머티리얼 변경
            else // 특수 상태로 변경해야 한다면
                await ChangeMaterialHelper.ChangeMaterial(_renderer, _emissionMaterial); // 머티리얼 변경
        }
    }
}

