using Sins.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Sins.Abilities
{
    public class AbilityButton : MonoBehaviour, IPointerClickHandler
    {
        public Ability Ability;

        public void OnPointerClick(PointerEventData eventData)
        {
            Hand.Instance.TakeMoveable(Ability);
        }
    }
}