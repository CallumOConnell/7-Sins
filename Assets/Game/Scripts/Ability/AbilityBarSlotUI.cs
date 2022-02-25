using UnityEngine;
using UnityEngine.UI;

namespace Sins.Abilities
{
    public class AbilityBarSlotUI : MonoBehaviour
    {
        [SerializeField]
        private Image _background;

        public void ShowCooldown(float cooldown)
        {
            _background.fillAmount = 0;

            LeanTween.value(gameObject, 0, 1, cooldown).setOnUpdate((float value) =>
            {
                _background.fillAmount = value;
            });
        }
    }
}