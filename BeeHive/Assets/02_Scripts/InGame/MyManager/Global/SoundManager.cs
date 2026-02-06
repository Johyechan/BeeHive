using InGame.MyManager.Enum;
using MyUtil;
using MyUtil.Interface;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace InGame.MyManager.Global
{
    // 작성자: 조혜찬
    // 사운드 매니저
    public class SoundManager : MonoSingleton<SoundManager>
    {
        [SerializeField] private List<SFXData> _sfxList = new List<SFXData>(); // 효과음 리스트

        private Dictionary<SFXType, AudioSource> _sfxMap = new Dictionary<SFXType, AudioSource>(); // 효과음 맵

        private const string MASTER_KEY = "Master"; // PlayerPrefs 마스터 볼륨 키
        private const string BGM_KEY = "BGM"; // PlayerPrefs bgm 볼륨 키
        private const string SFX_KEY = "SFX"; // PlayerPrefs sfx 볼륨 키

        public float MasterVolume { get => PlayerPrefs.GetFloat(MASTER_KEY, 1f); set => PlayerPrefs.SetFloat(MASTER_KEY, value); } // 마스터 프로퍼티

        public float BgmVolume { get => PlayerPrefs.GetFloat(BGM_KEY, 1f); set => PlayerPrefs.SetFloat(BGM_KEY, value); } // 배경음악 프로퍼티

        public float SfxVolume { get => PlayerPrefs.GetFloat(SFX_KEY, 1f); set => PlayerPrefs.SetFloat(SFX_KEY, value); } // 효과음 프로퍼티

        private bool _isFirstStart = true; // 처음 시작 여부
        public bool IsFirstStart { get => _isFirstStart; set => _isFirstStart = value; } // 처음 시작 여부 프로퍼티

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
// 마지막 작성 일자: 2026.02.06