using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Sins.UI;

namespace Sins.Abilities
{
    public class SelectedAbilitySlot : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        private AbilityBar _abilityBar;

        [SerializeField]
        private int _slotIndex = 0;

        private Image _icon;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                if (Hand.Instance.Moveable != null)
                {
                    _abilityBar.SetAbilityBarSlot(Hand.Instance.Moveable as Ability, _slotIndex);

                    _icon.sprite = Hand.Instance.Put().Icon;
                }
            }
        }

        private void Awake() => _icon = transform.GetChild(1).GetComponent<Image>();
    }
}