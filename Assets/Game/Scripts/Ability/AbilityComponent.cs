using UnityEngine;
using UnityEngine.UI;

namespace Sins.Abilities
{
    public class AbilityComponent : MonoBehaviour
    {
        private Button _button;

        public Ability Ability;

        private void Awake()
        {
            _button = GetComponent<Button>();

            if (_button != null)
            {
                _button.onClick.AddListener(AbilitySelected);
            }
        }

        public void AbilitySelected() => SelectedAbilitySlot.Instance.SetAbility(Ability);
    }
}