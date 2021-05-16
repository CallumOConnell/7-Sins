using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Sins.UI
{
    public class CloseButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        private Image _icon;

        [SerializeField]
        private GameObject _homeMenu;

        [SerializeField]
        private CanvasGroup _optionsMenu;

        public void OnPointerEnter(PointerEventData eventData)
        {
            LeanTween.value(gameObject, 0f, 1f, 0.5f).setOnUpdate((float value) =>
            {
                var colour = _icon.color;

                colour.a = value;

                _icon.color = colour;
            });
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            LeanTween.value(gameObject, 1f, 0f, 0f).setOnUpdate((float value) =>
            {
                var colour = _icon.color;

                colour.a = value;

                _icon.color = colour;
            });
        }

        public void Exit()
        {
            _optionsMenu.alpha = 0;
            _optionsMenu.interactable = false;
            _optionsMenu.blocksRaycasts = false;

            _homeMenu.SetActive(true);
        }
    }
}