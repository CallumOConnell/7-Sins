using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sins.Abilities
{
    public class AbilityBarUI : MonoBehaviour
    {
        [SerializeField]
        private GameObject _abilitySlotPrefab;

        [SerializeField]
        private AbilityBar _abilityBar;

        private Ability[] _activeAbilities;

        private List<GameObject> _abilitySlots = new List<GameObject>();

        private void Awake()
        {
            for (var i = 0; i < 3; i++)
            {
                if (_abilitySlotPrefab != null)
                {
                    var abilitySlot = Instantiate(_abilitySlotPrefab, transform);

                    _abilitySlots.Add(abilitySlot);
                }
            }
        }

        public void SetAbilityBarType(Ability[] abilityBar)
        {
            _activeAbilities = abilityBar;

            for (var i = 0; i < _activeAbilities.Length; i++)
            {
                var ability = _activeAbilities[i];

                if (ability != null)
                {
                    UpdateAbilitySlot(i);
                }
            }
        }

        public void UpdateAbilitySlot(int slotIndex)
        {
            if (_activeAbilities != null && _activeAbilities.Length > 0)
            {
                var abilitySlot = _abilitySlots[slotIndex];

                var abilitySlotUI = abilitySlot.GetComponent<AbilityBarSlotUI>();

                var ability = _activeAbilities[slotIndex];

                abilitySlot.transform.GetChild(0).GetComponent<Image>().sprite = ability.Icon;

                ability.OnAbilityUsed.AddListener(cooldown => abilitySlotUI.ShowCooldown(cooldown));
            }
        }
    }
}