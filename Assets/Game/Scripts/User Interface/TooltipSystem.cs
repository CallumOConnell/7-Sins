using UnityEngine;

namespace Sins.UI
{
    public class TooltipSystem : MonoBehaviour
    {
        public static TooltipSystem Instance;

        public Tooltip Tooltip;

        private void Awake() => Instance = this;

        public static void Show(string content, string header = "")
        {
            Instance.Tooltip.SetText(content, header);

            Instance.Tooltip.gameObject.SetActive(true);
        }

        public static void Hide() => Instance.Tooltip.gameObject.SetActive(false);
    }
}