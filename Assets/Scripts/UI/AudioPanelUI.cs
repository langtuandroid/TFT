// ************ @autor: Álvaro Repiso Romero *************
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UI
{
    public class AudioPanelUI : MonoBehaviour
    {
        [SerializeField] private Slider _musicVolumeSlider;
        [SerializeField] private Slider _sfxVolumeSlider;

        private AudioSpeaker _audioSpeaker;
        private OptionsSave  _optionsSave;

        private void Start()
        {
            _audioSpeaker = ServiceLocator.GetService<AudioSpeaker>();
            _optionsSave  = ServiceLocator.GetService<OptionsSave>();
            InitAudioSettings();
            SetSliderEvents();
        }

        private void SetSliderEvents()
        {
            _musicVolumeSlider.onValueChanged.AddListener( delegate { ChangeMusicVolume(); } );
            _sfxVolumeSlider.onValueChanged.AddListener( delegate { ChangeSfxVolume(); } );
        }

        private void InitAudioSettings()
        {
            _musicVolumeSlider.value = _audioSpeaker.MusicVolume() * _musicVolumeSlider.maxValue;
            _sfxVolumeSlider.value   = _audioSpeaker.SfxVolume()   * _sfxVolumeSlider.maxValue;
            _optionsSave.musicVolume = _audioSpeaker.MusicVolume();
            _optionsSave.sfxVolume   = _audioSpeaker.SfxVolume();
        }

        private void ChangeMusicVolume()
        {
            float volume = _musicVolumeSlider.value / _musicVolumeSlider.maxValue;
            _audioSpeaker.SetMusicVolume( volume );
            _optionsSave.musicVolume = volume;
        }        
        
        private void ChangeSfxVolume()
        {
            float volume = _sfxVolumeSlider.value / _sfxVolumeSlider.maxValue;
            _audioSpeaker.SetSfxVolume( volume );
            _optionsSave.sfxVolume = volume;
        }
    }
}