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
            
            _activeAbilities = _abilityBar.Abilities;
        }

        public void UpdateAbilitySlot(int slotIndex)
        {
            var abilitySlot = _abilitySlots[slotIndex];

            var abilitySlotUI = abilitySlot.GetComponent<AbilityBarSlotUI>();

            var ability = _activeAbilities[slotIndex];

            abilitySlot.transform.GetChild(0).GetComponent<Image>().sprite = ability.Icon;

            ability.OnAbilityUsed.AddListener(cooldown => abilitySlotUI.ShowCooldown(cooldown));
        }
    }
}