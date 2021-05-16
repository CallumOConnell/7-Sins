using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Sins.UI
{
    public class ToggleSwitch : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField]
        private bool _defaultState = false;

        [SerializeField]
        private RectTransform _toggleIndicator;

        [SerializeField]
        private Image _backgroundImage;

        [SerializeField]
        private Color _indicatorColourOn;

        [SerializeField]
        private Color _indicatorColourOff;

        [SerializeField]
        private Color _backgroundColourOn;

        [SerializeField]
        private Color _backgroundColourOff;

        [SerializeField]
        private AudioClip _toggleSound;

        public bool State { get; private set; }

        public delegate void ValueChanged(bool value);

        public event ValueChanged valueChanged;

        private AudioSource _audioSource;

        public void OnPointerDown(PointerEventData eventData)
        {
            Toggle(!State);
        }

        private void Start()
        {
            State = _defaultState;

            _audioSource = GetComponent<AudioSource>();
        }

        private void Toggle(bool value)
        {

            SaveState(value);

            if (_audioSource != null && _toggleSound != null)
            {
                _audioSource.PlayOneShot(_toggleSound);
            }

            if (valueChanged != null)
            {
                valueChanged(value);
            }
        }

        private void SaveState(bool value)
        {
            var converted = value ? 1 : 0;

            PlayerPrefs.SetInt(gameObject.name + "Switch", converted);
        }

        private void AnimateSwitch(bool value)
        {

        }
    }
}