using MyUtil;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;

namespace InGame.MyManager
{
    // 작성자: 조혜찬
    // GPU 관련 싱글톤 매니저
    public class GpuManager : MonoSingleton<GpuManager>
    {
        public bool IsReady { get; private set; } // 외부에서는 접근만 가능하고 변경은 내부에서 해야하는 프로퍼티 (GPU 준비 여부)

        protected override async void Awake()
        {
            base.Awake();

            if (IsReady) return; // GPU가 준비 되어 있다면 반환

            while (SystemInfo.graphicsDeviceType == GraphicsDeviceType.Null) // GPU가 할당 되지 않았다면
            {
                await Task.Yield(); // 한 프레임 대기
            }

            IsReady = true;
        }
    }
}
// 마지막 작성 일자: 2025.10.29