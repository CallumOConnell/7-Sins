using UnityEngine;
using UnityEngine.EventSystems;

namespace Sins.UI
{
    public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public string Header;

        [Multiline]
        public string Content;

        private static LTDescr _delay;

        public void OnPointerEnter(PointerEventData eventData)
        {
            _delay = LeanTween.delayedCall(0.5f, () =>
            {
                TooltipSystem.Show(Content, Header);
            });
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            LeanTween.cancel(_delay.uniqueId);

            TooltipSystem.Hide();
        }
    }
}