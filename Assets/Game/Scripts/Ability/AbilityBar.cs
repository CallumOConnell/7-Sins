using UnityEngine;

namespace Sins.Abilities
{
    public class AbilityBar : MonoBehaviour
    {
        [HideInInspector]
        public Ability[] MeleeAbilities = new Ability[3];

        [HideInInspector]
        public Ability[] RangedAbilities = new Ability[3];

        [HideInInspector]
        public Ability[] MagicAbilities = new Ability[3];

        [HideInInspector]
        public Ability[] CurrentBar;

        [HideInInspector]
        public AbilityType CurrentBarType;

        [SerializeField]
        private AbilityBarUI _abilityBarUI;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                TryActivateAbility(0);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                TryActivateAbility(1);
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                TryActivateAbility(2);
            }
        }

        private void TryActivateAbility(int abilityType)
        {
            if (CurrentBar != null && CurrentBar.Length > 0)
            {
                var ability = CurrentBar[abilityType];

                if (ability != null)
                {
                    if (ability.CanUse)
                    {
                        ability.ActivateAbility();
                    }
                }
            }
        }

        public void SetActiveBar(AbilityType type)
        {
            switch (type)
            {
                case AbilityType.Melee:
                {
                    CurrentBar = MeleeAbilities;

                    break;
                }

                case AbilityType.Ranged:
                {
                    CurrentBar = RangedAbilities;

                    break;
                }

                case AbilityType.Magic:
                {
                    CurrentBar = MagicAbilities;

                    break;
                }
            }

            CurrentBarType = type;

            _abilityBarUI.SetAbilityBarType(CurrentBar);
        }

        public void SetAbilityBarSlot(Ability ability, int slotIndex)
        {
            switch (ability.Type)
            {
                case AbilityType.Melee:
                {
                    MeleeAbilities[slotIndex] = ability;

                    break;
                }

                case AbilityType.Ranged:
                {
                    RangedAbilities[slotIndex] = ability;

                    break;
                }

                case AbilityType.Magic:
                {
                    MagicAbilities[slotIndex] = ability;

                    break;
                }
            }

            if (CurrentBarType == ability.Type)
            {
                _abilityBarUI.UpdateAbilitySlot(slotIndex);
            }
        }
    }
}