using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UI
{
    public class AudioPanelUI : MonoBehaviour
    {
        [SerializeField] private Slider _musicVolumeSlider;
        [SerializeField] private Slider _sfxVolumeSlider;
        [SerializeField] private Slider _uIVolumeSlider;

        private void Start()
        {
            InitSliders();
            SetSliderEvents();
        }

        private void SetSliderEvents()
        {
            _musicVolumeSlider.onValueChanged.AddListener( delegate { ChangeMusicVolume(); } );
            _sfxVolumeSlider.onValueChanged.AddListener( delegate { ChangeSfxVolume(); } );
            //_uIVolumeSlider.onValueChanged.AddListener( delegate { ChangeUIVolume(); } );
        }

        private void InitSliders()
        {
            _musicVolumeSlider.value = AudioManager.Instance.MusicVolume();
            _sfxVolumeSlider.value = AudioManager.Instance.SfxVolume();
            //_uIVolumeSlider.value = AudioManager.Instance.UIVolume();
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
        
        private void ChangeUIVolume()
        {
            float volume = _uIVolumeSlider.value;
            AudioManager.Instance.SetUIVolume( volume );
        }
    }
}