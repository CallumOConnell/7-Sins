using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Michsky.UI.Dark
{
    public class SwitchManager : MonoBehaviour, IPointerDownHandler
    {
        public bool State { get; private set; }

        [System.Serializable]
        public class UnityEventBool : UnityEvent<bool> { }

        public UnityEventBool OnValueChanged;

        private Animator _animator;

        private void OnEnable()
        {
            var state = PlayerPrefs.GetInt(gameObject.name + "Switch", 0) == 1;

            Toggle(state);
        }

        private void Awake()
        {
            _animator = GetComponent<Animator>();

            var state = PlayerPrefs.GetInt(gameObject.name + "Switch", 0) == 1;

            Toggle(state);
        }

        private void Toggle(bool value)
        {
            State = value;

            OnValueChanged.Invoke(value);

            if (value)
            {
                _animator.Play("Switch On");

                PlayerPrefs.SetInt(gameObject.name + "Switch", 1);
            }
            else
            {
                _animator.Play("Switch Off");

                PlayerPrefs.SetInt(gameObject.name + "Switch", 0);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Toggle(!State);
        }
    }
}