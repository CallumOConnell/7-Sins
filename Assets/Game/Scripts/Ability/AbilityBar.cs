using UnityEngine;

namespace Sins.Abilities
{
    public class AbilityBar : MonoBehaviour
    {
        [HideInInspector]
        public Ability[] Abilities = new Ability[3];

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
            var ability = Abilities[abilityType];

            if (ability != null)
            {
                if (ability.CanUse)
                {
                    ability.ActivateAbility();
                }
            }
        }
    }
}