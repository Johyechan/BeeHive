using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;

namespace InGame.MyManager.Boot
{
    // 작성자: 조혜찬
    // GPU 관련 싱글톤 매니저
    public class GpuChecker: CheckerBase
    {
        [SerializeField] private float _waitTime; // 대기 시간

        protected override async Task<bool> Check()
        {
            float currentTime = 0;

            while (SystemInfo.graphicsDeviceType == GraphicsDeviceType.Null) // GPU가 할당 되지 않았다면
            {
                currentTime += Time.unscaledDeltaTime;

                if(currentTime > _waitTime) // 대기 시간을 넘어갔다면
                {
                    NetworkManager.Instance.Socket.Emit("debug", "GPU 할당 안된다");
                    Application.Quit(); // 강제 종료
                    break;
                }

                await Task.Yield(); // 한 프레임 대기
            }

            await Task.CompletedTask;
            return true;
        }
    }
}
// 마지막 작성 일자: 2025.01.06