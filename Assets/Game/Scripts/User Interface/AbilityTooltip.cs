using Sins.Abilities;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Sins.UI
{
    public class AbilityTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private Ability _ability;

        private void Start() => _ability = GetComponent<AbilityButton>().Ability;

        public void OnPointerEnter(PointerEventData eventData) => TooltipSystem.Show(_ability.Description, _ability.Title);

        public void OnPointerExit(PointerEventData eventData) => TooltipSystem.Hide();
    }
}