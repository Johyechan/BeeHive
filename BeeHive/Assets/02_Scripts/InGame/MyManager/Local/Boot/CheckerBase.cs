using System.Threading.Tasks;
using UnityEngine;

namespace InGame.MyManager.Local.Boot
{
    // 작성자: 조혜찬
    // 검증 클래스의 부모 클래스
    public abstract class CheckerBase
    {
        protected abstract Task<bool> Check();

        public async Task<bool> Init()
        {
            return await Check();
        }
    }
}
// 마지막 작성 일자: 2026.02.03