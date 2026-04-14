using InGame.MyEnum;
using InGame.MyManager;
using InGame.MyManager.Global;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace InGame.MySystem
{
    // 작성자: 조혜찬
    // 세팅 UI 클래스
    public class SettingUI : MonoBehaviour
    {
        [SerializeField] private AudioMixer _audioMixer;

        [SerializeField] private Slider _masterSoundSlider; // 모든 볼륨 조절 슬라이더
        [SerializeField] private Slider _bgmSoundSlider; // 배경음악 볼륨 조절 슬라이더
        [SerializeField] private Slider _sfxSoundSlider; // 효과음 볼륨 조절 슬라이더

        [SerializeField] private TMP_Text _masterSoundVolumePercentage; // 마스터 볼륨 퍼센트
        [SerializeField] private TMP_Text _bgmSoundVolumePercentage; // 배경음악 볼륨 퍼센트
        [SerializeField] private TMP_Text _sfxSoundVolumePercentage; // 효과음 볼륨 퍼센트

        [SerializeField] private Toggle _koreaToggle; // 한글 토글
        [SerializeField] private Toggle _englishToggle; // 영어 토글

        [SerializeField] private TMP_Dropdown _resolutionDropdown; // 해상도 드롭다운

        private void Awake()
        {
            _masterSoundSlider.onValueChanged.AddListener(SetMasterVolume);
            _bgmSoundSlider.onValueChanged.AddListener(SetBGMVolume);
            _sfxSoundSlider.onValueChanged.AddListener(SetSFXVolume);
        }

        private void Start()
        {
            _masterSoundSlider.value = SoundManager.Instance.MasterVolume;
            _bgmSoundSlider.value = SoundManager.Instance.BgmVolume;
            _sfxSoundSlider.value = SoundManager.Instance.SfxVolume;

            SetMasterVolume(SoundManager.Instance.MasterVolume);
            SetBGMVolume(SoundManager.Instance.BgmVolume);
            SetSFXVolume(SoundManager.Instance.SfxVolume);

            SetLanguageToggle();

            SetResolution();
        }

        private void SetResolution()
        {
            _resolutionDropdown.value = UIManager.Instance.CurrentResolutionIndex; // 선택된 인덱스를 현재 선택한 해상도 인덱스로 변경
            _resolutionDropdown.RefreshShownValue(); // 보이는 UI 변경
        }

        private void SetLanguageToggle()
        {
            switch(LanguageManager.Instance.CurrentLanguage)
            {
                case LanguageType.Korea:
                    _koreaToggle.isOn = true;
                    break;
                case LanguageType.English:
                    _englishToggle.isOn = true;
                    break;
            }
        }

        // dB = 20 X log10(볼륨비) 

        // 모든 볼륨 세팅
        private void SetMasterVolume(float volume)
        {
            SoundManager.Instance.MasterVolume = volume;
            if (volume <= 0.0001f) // 최소값이 0.0001인 이유는 0은 log10으로 계산했을 때 무한대가 나와 nan이 나와 버그 발생 가능
            {
                _audioMixer.SetFloat("Master", -80); // 마스터 볼륨 -80db로 할당 (-80db는 소리가 안남)
                _masterSoundVolumePercentage.text = "0%";
            }
            else
            {
                _audioMixer.SetFloat("Master", Mathf.Log10(volume) * 20f); // 마스터 볼륨 할당 (volume이 1일 때: 0 db - 원래 소리 크기)
                _masterSoundVolumePercentage.text = $"{Mathf.Ceil(volume * 100f)}%";
            }
        }

        // 배경 음악 세팅 함수
        private void SetBGMVolume(float volume)
        {
            SoundManager.Instance.BgmVolume = volume;
            if (volume <= 0.0001f)
            {
                _audioMixer.SetFloat("BGM", -80);
                _bgmSoundVolumePercentage.text = "0%";
            }
            else
            {
                _audioMixer.SetFloat("BGM", Mathf.Log10(volume) * 20f);
                _bgmSoundVolumePercentage.text = $"{Mathf.Ceil(volume * 100f)}%";
            }
        }

        // 효과음 세팅 함수
        private void SetSFXVolume(float volume)
        {
            SoundManager.Instance.SfxVolume = volume;
            if (volume <= 0.0001f)
            {
                _audioMixer.SetFloat("SFX", -80);
                _sfxSoundVolumePercentage.text = "0%";
            }
            else
            {
                _audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20f);
                _sfxSoundVolumePercentage.text = $"{Mathf.Ceil(volume * 100f)}%";
            }
        }
    }
}
// 마지막 작성 일자: 2026.04.14