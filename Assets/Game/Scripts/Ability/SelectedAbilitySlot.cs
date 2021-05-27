using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Sins.Abilities
{
    public class SelectedAbilitySlot : MonoBehaviour
    {
        [SerializeField]
        private AbilityBar _abilityBar;

        [SerializeField]
        private AbilityBarUI _abilityBarUI;

        [SerializeField]
        private GameObject _meleeSlot;

        [SerializeField]
        private GameObject _rangedSlot;

        [SerializeField]
        private GameObject _magicSlot;

        public static SelectedAbilitySlot Instance;

        public Ability Ability { get; set; }

        private void Awake()
        {
            Instance = this;
        }

        public void SetAbility(Ability newAbility)
        {
            Ability = newAbility;

            GameObject selectedSlot = null;

            switch (newAbility.Type)
            {
                case AbilityType.Melee:
                {
                    selectedSlot = _meleeSlot;

                    break;
                }
                case AbilityType.Ranged:
                {
                    selectedSlot = _rangedSlot;

                    break;
                }
                case AbilityType.Magic:
                {
                    selectedSlot = _magicSlot;

                    break;
                }
            }

            if (selectedSlot != null)
            {
                selectedSlot.transform.GetChild(1).GetComponent<Image>().sprite = newAbility.Icon;
                selectedSlot.transform.GetChild(2).GetComponent<TMP_Text>().text = newAbility.Title;
            }

            if (_abilityBar != null)
            {
                _abilityBar.Abilities[(int)newAbility.Type] = newAbility;
            }

            if (_abilityBarUI != null)
            {
                _abilityBarUI.UpdateAbilitySlot((int)newAbility.Type);
            }
        }
    }
}