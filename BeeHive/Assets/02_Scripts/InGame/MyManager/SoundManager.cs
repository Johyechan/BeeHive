using MyUtil;
using UnityEngine;

namespace InGame.MyManager
{
    // 작성자: 조혜찬
    // 사운드 매니저
    public class SoundManager : MonoSingleton<SoundManager>
    {
        private float _masterVolume = 1; // 마스터
        public float MasterVolume { get => _masterVolume; set => _masterVolume = value; } // 마스터 프로퍼티

        private float _bgmVolume = 1; // 배경음악
        public float BgmVolume { get => _bgmVolume; set => _bgmVolume = value; } // 배경음악 프로퍼티

        private float _sfxVolume = 1; // 효과음
        public float SfxVolume { get => _sfxVolume; set => _sfxVolume = value; } // 효과음 프로퍼티
    }
}
// 마지막 작성 일자: 2025.11.11