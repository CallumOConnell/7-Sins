using System.Collections;
using UnityEngine;

namespace Sins.Character
{
    public class PlayerCombat : CharacterCombat
    {
        [SerializeField]
        private float _attackDelay = 1.167f;

        [SerializeField]
        private Animator _animator;

        private float _attackCooldown = 0f;

        private PlayerStats _playerStats;

        private void Start() => _playerStats = GetComponent<PlayerStats>();

        private void Update() => _attackCooldown -= Time.deltaTime;

        private IEnumerator DoDamage(CharacterStats targetStats, float delay)
        {
            yield return new WaitForSeconds(delay);

            if (targetStats != null)
            {
                targetStats.Damage(_playerStats.AttackDamage.GetValue());
            }
        }

        public override void Attack(CharacterStats targetStats)
        {
            base.Attack(targetStats);

            if (_attackCooldown <= 0f)
            {
                _animator.SetTrigger("attack");

                StartCoroutine(DoDamage(targetStats, _attackDelay));

                _attackCooldown = 1f / _playerStats.AttackSpeed.GetValue();
            }
        }
    }
}