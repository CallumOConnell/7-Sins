using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Sins.UI
{
    public class NavigationButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        private Color _keyTextIdle;

        [SerializeField]
        private Color _keyTextHover;

        [SerializeField]
        private Color _actionTextIdle;

        [SerializeField]
        private Color _actionTextHover;

        [SerializeField]
        private TextMeshProUGUI _keyText;

        [SerializeField]
        private TextMeshProUGUI _actionText;

        [SerializeField]
        private TabGroup _tabGroup;

        [SerializeField]
        private CanvasGroup _optionsMenu;

        [SerializeField]
        private GameObject _homeMenu;

        public void OnPointerEnter(PointerEventData eventData)
        {
            _keyText.color = _keyTextHover;
            _actionText.color = _actionTextHover;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _keyText.color = _keyTextIdle;
            _actionText.color = _actionTextIdle;
        }

        public void SelectNextPage()
        {
            _tabGroup.NextTab();
        }

        public void SelectPreviousPage()
        {
            _tabGroup.PreviousTab();
        }

        public void ExitToHome()
        {
            _optionsMenu.alpha = 0f;
            _optionsMenu.interactable = false;
            _optionsMenu.blocksRaycasts = false;

            _homeMenu.SetActive(true);
        }
    }
}