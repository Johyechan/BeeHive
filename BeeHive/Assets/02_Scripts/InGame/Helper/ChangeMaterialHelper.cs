using InGame.MyManager;
using System.Threading.Tasks;
using UnityEngine;

namespace InGame.Helper
{
    // 작성자: 조혜찬
    // Material을 변경하는 것을 돕는 정적 클래스
    public class ChangeMaterialHelper
    {
        // 랜더러를 통해 실시간으로 머티리얼을 변경하는 함수
        public static void ChangeMaterial(Renderer renderer, Material material)
        {
            renderer.material = material; // 실시간으로 머티리얼의 변경을 보여주기 위해서 랜더러의 머티리얼을 변경
        }
    }
}
// 마지막 작성 일자: 2026.01.14