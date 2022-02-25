using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sins.Character;

namespace Sins.Abilities
{
    public class AbilitiesUI : MonoBehaviour
    {
        [SerializeField]
        private Ability[] _availableAbilities;

        [SerializeField]
        private GameObject _abilityPrefab;

        private void Start()
        {
            if (_availableAbilities.Length > 0)
            {
                foreach (var ability in _availableAbilities)
                {
                    if (ability != null)
                    {
                        if (Player.Instance.Level >= ability.LevelRequirement)
                        {
                            if (_abilityPrefab != null)
                            {
                                var abilitySlot = Instantiate(_abilityPrefab, transform);

                                abilitySlot.GetComponent<AbilityButton>().Ability = ability;

                                abilitySlot.transform.GetChild(0).GetComponent<Image>().sprite = ability.Icon;
                                abilitySlot.transform.GetChild(1).GetComponent<TMP_Text>().text = ability.Title;
                            }
                        }
                    }
                }
            }
        }
    }
}