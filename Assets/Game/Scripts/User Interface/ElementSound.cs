using UnityEngine;
using UnityEngine.EventSystems;

namespace Sins.UI
{
    public class ElementSound : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
    {
        [SerializeField]
        private AudioSource _audioSource;

        [SerializeField]
        private AudioClip _clickSound;

        [SerializeField]
        private AudioClip _hoverSound;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_audioSource != null && _clickSound != null)
            {
                _audioSource.PlayOneShot(_clickSound);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_audioSource != null && _hoverSound != null)
            {
                _audioSource.PlayOneShot(_hoverSound);
            }
        }
    }
}