using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Sins.UI
{
    public class QualityManager : MonoBehaviour
    {
        [Header("Audio"), Space]

        [SerializeField]
        private AudioMixer _masterMixer;

        [SerializeField]
        private Slider _masterSlider;

        [SerializeField]
        private Slider _musicSlider;

        [SerializeField]
        private Slider _sfxSlider;

        [SerializeField]
        private Slider _uiSlider;

        [Header("Resolution"), Space]

        [SerializeField]
        private Dropdown _resolutionDropDown;

        private Resolution[] _resolutions;

        private void Start()
        {
            var masterVolume = Mathf.Log10(PlayerPrefs.GetFloat("MasterVolume")) * 20;

            _masterMixer.SetFloat("Master", masterVolume);

            var musicVolume = Mathf.Log10(PlayerPrefs.GetFloat("MusicVolume")) * 20;

            _masterMixer.SetFloat("Music", musicVolume);

            var sfxVolume = Mathf.Log10(PlayerPrefs.GetFloat("SFXVolume")) * 20;

            _masterMixer.SetFloat("SFX", sfxVolume);

            var uiVolume = Mathf.Log10(PlayerPrefs.GetFloat("UIVolume")) * 20;

            _masterMixer.SetFloat("UI", uiVolume);

            _resolutions = Screen.resolutions;

            _resolutionDropDown.ClearOptions();

            var options = new List<string>();

            var currentResolutionIndex = 0;

            for (var i = 0; i < _resolutions.Length; i++)
            {
                var option = _resolutions[i].width + " x " + _resolutions[i].height;

                options.Add(option);

                if (_resolutions[i].width == Screen.currentResolution.width && _resolutions[i].height == Screen.currentResolution.height)
                {
                    currentResolutionIndex = i;
                }
            }

            _resolutionDropDown.AddOptions(options);
            _resolutionDropDown.value = currentResolutionIndex;
            _resolutionDropDown.RefreshShownValue();
        }

        private void SetVolume(string name, float value)
        {
            PlayerPrefs.SetFloat(name + "VolumeSliderPosition", value);

            float volume = Mathf.Log10(value) * 20;

            _masterMixer.SetFloat(name, volume);

            PlayerPrefs.SetFloat(name + "Volume", volume);
        }

        public void SetResolution(int resolutionIndex)
        {
            var resolution = _resolutions[resolutionIndex];

            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        }

        public void SetWindowMode(int mode)
        {
            switch (mode)
            {
                case 0: // Fullscreen
                {
                    Screen.fullScreen = true;
                    Screen.fullScreenMode = FullScreenMode.FullScreenWindow;

                    break;
                }
                case 1: // Borderless
                {
                    Screen.fullScreenMode = FullScreenMode.MaximizedWindow;

                    break;
                }
                case 2: // Windowed
                {
                    Screen.fullScreen = false;
                    Screen.fullScreenMode = FullScreenMode.Windowed;

                    break;
                }
            }
        }

        public void SetVync(int index)
        {
            QualitySettings.vSyncCount = index;
        }

        public void SetAntiAlisasingQuality(int index)
        {
            QualitySettings.antiAliasing = index;
        }

        public void SetShadowResolutionQuality(int index)
        {
            switch (index)
            {
                case 0:
                {
                    QualitySettings.shadowResolution = ShadowResolution.Low;

                    break;
                }
                case 1:
                {
                    QualitySettings.shadowResolution = ShadowResolution.Medium;

                    break;
                }
                case 2:
                {
                    QualitySettings.shadowResolution = ShadowResolution.High;

                    break;
                }
                case 3:
                {
                    QualitySettings.shadowResolution = ShadowResolution.VeryHigh;

                    break;
                }
            }
        }

        public void SetTextureQuality(int index)
        {
            QualitySettings.masterTextureLimit = index;
        }

        public void SetShadowQuality(int index)
        {
            switch (index)
            {
                case 0:
                {
                    QualitySettings.shadows = ShadowQuality.Disable;

                    break;
                }
                case 1:
                {
                    QualitySettings.shadows = ShadowQuality.HardOnly;

                    break;
                }
                case 2:
                {
                    QualitySettings.shadows = ShadowQuality.All;

                    break;
                }
            }
        }

        public void SetShadowCascasdsQuality(int index)
        {
            QualitySettings.shadowCascades = index;
        }

        public void SetSoftParticleQuality(bool state)
        {
            QualitySettings.softParticles = state;
        }

        public void SetReflectionQuality(bool state)
        {
            QualitySettings.realtimeReflectionProbes = state;
        }

        public void SetQualityPreset(int index)
        {
            QualitySettings.SetQualityLevel(index);
        }

        public void SetAnisotropicFilteringQuality(int index)
        {
            switch (index)
            {
                case 0:
                {
                    QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;

                    break;
                }
                case 1:
                {
                    QualitySettings.anisotropicFiltering = AnisotropicFiltering.Enable;

                    break;
                }
                case 2:
                {
                    QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable;

                    break;
                }
            }
        }

        public void SetMasterVolume(float value)
        {
            SetVolume("Master", value);
        }

        public void SetMusicVolume(float value)
        {
            SetVolume("Music", value);
        }

        public void SetSFXVolume(float value)
        {
            SetVolume("SFX", value);
        }

        public void SetUIVolume(float value)
        {
            SetVolume("UI", value);
        }
    }
}