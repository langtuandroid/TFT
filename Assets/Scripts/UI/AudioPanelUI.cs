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
            _musicVolumeSlider.value = AudioManager.Instance.MusicVolume();
            _sfxVolumeSlider.value = AudioManager.Instance.SfxVolume();
        }

        private void ChangeMusicVolume()
        {
            float volume = _musicVolumeSlider.value;
            AudioManager.Instance.SetMusicVolume( volume );
        }        
        
        private void ChangeSfxVolume()
        {
            float volume = _sfxVolumeSlider.value;
            AudioManager.Instance.SetSfxVolume( volume );
        }
    }
}