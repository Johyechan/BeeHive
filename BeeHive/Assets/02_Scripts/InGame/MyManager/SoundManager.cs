using MyUtil;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace InGame.MyManager
{
    // 작성자: 조혜찬
    // 사운드 매니저
    public class SoundManager : MonoSingleton<SoundManager>
    {
        [SerializeField] private List<SFXData> _sfxList = new List<SFXData>(); // 효과음 리스트

        private Dictionary<SFXType, AudioSource> _sfxMap = new Dictionary<SFXType, AudioSource>(); // 효과음 맵

        private float _masterVolume = 1; // 마스터
        public float MasterVolume { get => _masterVolume; set => _masterVolume = value; } // 마스터 프로퍼티

        private float _bgmVolume = 1; // 배경음악
        public float BgmVolume { get => _bgmVolume; set => _bgmVolume = value; } // 배경음악 프로퍼티

        private float _sfxVolume = 1; // 효과음
        public float SfxVolume { get => _sfxVolume; set => _sfxVolume = value; } // 효과음 프로퍼티

        protected override void Awake()
        {
            base.Awake();

            Init();
        }

        // 초기화 함수
        private void Init()
        {
            foreach(var data in  _sfxList) // 효과음 리스트 순회
            {
                SFXType type = data.sfxType;
                AudioSource audioSource = data.audioSource;
                if(!_sfxMap.ContainsKey(type)) // 효과음 맵에 중복 키가 없다면
                {
                    _sfxMap.Add(type, audioSource); // 효과음 맵에 추가
                }
            }
        }

        public void SFXPlay(SFXType type)
        {
            _sfxMap[type].Play(); // 효과음 타입에 맞는 사운드 실행
        }
    }
}
// 마지막 작성 일자: 2025.11.14