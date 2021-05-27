using Sins.Character;
using System.Collections;
using UnityEngine;

namespace Sins.Abilities
{
    public class BloodLustAbility : MonoBehaviour
    {
        [SerializeField]
        private Ability _ability;

        [SerializeField]
        private int _attackDamage;

        [SerializeField]
        private float _effectDuration;

        private PlayerStats _playerStats;

        private void Awake()
        {
            _ability.OnAbilityUsed.AddListener(cooldown => Use());

            _ability.CanUse = true;

            _playerStats = GetComponent<PlayerStats>();
        }

        private IEnumerator ResetDuration()
        {
            yield return new WaitForSeconds(_effectDuration);

            _playerStats.AttackDamage.RemoveModifier(_attackDamage);
        }

        public void Use()
        {
            _playerStats.AttackDamage.AddModifier(_attackDamage);

            StartCoroutine(ResetDuration());
        }
    }
}