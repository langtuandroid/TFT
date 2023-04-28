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

        private void Start()
        {
            InitSliders();
            SetSliderEvents();
        }

        private void SetSliderEvents()
        {
            _musicVolumeSlider.onValueChanged.AddListener( delegate { ChangeMusicVolume(); } );
            _sfxVolumeSlider.onValueChanged.AddListener( delegate { ChangeSfxVolume(); } );
        }

        private void InitSliders()
        {
            _musicVolumeSlider.value = AudioManager.Instance.MusicVolume() * _musicVolumeSlider.maxValue;
            _sfxVolumeSlider.value = AudioManager.Instance.SfxVolume() * _sfxVolumeSlider.maxValue;
        }

        private void ChangeMusicVolume()
        {
            float volume = _musicVolumeSlider.value / _musicVolumeSlider.maxValue;
            AudioManager.Instance.SetMusicVolume( volume );
        }        
        
        private void ChangeSfxVolume()
        {
            float volume = _sfxVolumeSlider.value / _sfxVolumeSlider.maxValue;
            AudioManager.Instance.SetSfxVolume( volume );
        }
    }
}