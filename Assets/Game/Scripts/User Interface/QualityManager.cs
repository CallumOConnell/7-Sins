using System.Collections.Generic;
using System.Linq;
using TMPro;
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

        [SerializeField]
        private TMP_Text _masterText;

        [SerializeField]
        private TMP_Text _musicText;

        [SerializeField]
        private TMP_Text _effectsText;

        [SerializeField]
        private TMP_Text _uiText;

        [Header("Resolution"), Space]

        [SerializeField]
        private TMP_Dropdown _resolutionDropDown;

        private Resolution[] _resolutions;

        private void OnEnable()
        {
            InitialiseAudio();
            InitialiseDisplay();
        }

        public void InitialiseAudio()
        {
            _masterSlider.value = PlayerPrefs.GetFloat("MasterVolumeSliderPosition", 1);
            _musicSlider.value = PlayerPrefs.GetFloat("MusicVolumeSliderPosition", 1);
            _sfxSlider.value = PlayerPrefs.GetFloat("SFXVolumeSliderPosition", 1);
            _uiSlider.value = PlayerPrefs.GetFloat("UIVolumeSliderPosition", 1);

            _masterText.text = _masterSlider.value.ToString("F1");
            _musicText.text = _musicSlider.value.ToString("F1");
            _effectsText.text = _sfxSlider.value.ToString("F1");
            _uiText.text = _uiSlider.value.ToString("F1");
        }

        public void InitialiseDisplay()
        {
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

            var volume = Mathf.Log10(value) * 20;

            _masterMixer.SetFloat(name, volume);
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

        public void SetVync(int index) => QualitySettings.vSyncCount = index;

        public void SetAntiAlisasingQuality(int index) => QualitySettings.antiAliasing = index; // Value between 2 and 8 and disabled option.

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

        public void SetTextureQuality(int index) => QualitySettings.masterTextureLimit = index;

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

        public void SetShadowCascasdsQuality(int index) => QualitySettings.shadowCascades = index;

        public void SetSoftParticleQuality(int index) => QualitySettings.softParticles = index == 1;

        public void SetReflectionQuality(int index) => QualitySettings.realtimeReflectionProbes = index == 1;

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
            if (_masterSlider.IsActive())
            {
                SetVolume("Master", value);

                _masterText.text = value.ToString("F1");
            }
        }

        public void SetMusicVolume(float value)
        {
            if (_musicSlider.IsActive())
            {
                SetVolume("Music", value);

                _musicText.text = value.ToString("F1");
            }
        }

        public void SetSFXVolume(float value)
        {
            if (_sfxSlider.IsActive())
            {
                SetVolume("SFX", value);
            }

            _effectsText.text = value.ToString("F1");
        }

        public void SetUIVolume(float value)
        {
            if (_uiSlider.IsActive())
            {
                SetVolume("UI", value);

                _uiText.text = value.ToString("F1");
            }
        }

        [SerializeField]
        private int _pressurePlateIndex = 0;

        [SerializeField]
        private bool _firstPressurePlate = false;

        private Collider _collider;

        private BoxPuzzle _boxPuzzle;

        private void Awake()
        {
            _collider = GetComponent<Collider>();
            _boxPuzzle = GetComponent<BoxPuzzle>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_firstPressurePlate && !_boxPuzzle.IsActive[_pressurePlateIndex]) // Activate the first pressure plate if it is not active
            {
                _boxPuzzle.IsActive[_pressurePlateIndex] = true;
            }

            if (_boxPuzzle.IsActive[_pressurePlateIndex]) // Current pressure plate is active
            {
                _boxPuzzle.IsActive[_pressurePlateIndex + 1] = true;

                _collider.enabled = false;

                Debug.Log($"Box {_pressurePlateIndex + 1} is now active", gameObject);
            }
            else if (!_firstPressurePlate && !_boxPuzzle.IsActive[_pressurePlateIndex - 1]) // Previous pressure plate is not active
            {
                Debug.Log($"You need to active number {_pressurePlateIndex - 1} first before activating number {_pressurePlateIndex}", gameObject);
            }
        }
    }

    public class BoxPuzzle : MonoBehaviour
    {
        [SerializeField]
        private GameObject _door;

        public List<bool> IsActive = new List<bool>()
        {
            false,
            false,
            false
        };

        private void Update()
        {
            if (!IsActive.Contains(false))
            {
                _door.SetActive(false);
            }
        }
    }
}