using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using TMPro;

namespace Sins.UI
{
    [RequireComponent(typeof(Image))]
    public class TabButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
    {
        [SerializeField]
        private TabGroup _tabGroup;

        public UnityEvent OnTabSelected;
        public UnityEvent OnTabDeselected;

        public Image Background { get; set; }

        public TMP_Text Text { get; set; }

        public void OnPointerClick(PointerEventData eventData) => _tabGroup.OnTabSelected(this);

        public void OnPointerEnter(PointerEventData eventData) => _tabGroup.OnTabEnter(this);

        public void OnPointerExit(PointerEventData eventData) => _tabGroup.OnTabExit(this);

        public void Select()
        {
            if (OnTabSelected != null)
            {
                OnTabSelected.Invoke();
            }
        }

        public void Deselect()
        {
            if (OnTabDeselected != null)
            {
                OnTabDeselected.Invoke();
            }
        }

        private void Start()
        {
            Background = GetComponent<Image>();
            Text = GetComponentInChildren<TextMeshProUGUI>();

            if (_tabGroup != null)
            {
                _tabGroup.Subcribe(this);
            }
        }
    }
}