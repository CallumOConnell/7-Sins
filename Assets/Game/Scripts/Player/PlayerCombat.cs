using Sins.Abilities;
using Sins.Inventory;
using System.Collections;
using UnityEngine;

namespace Sins.Character
{
    public class PlayerCombat : CharacterCombat
    {
        [SerializeField]
        private GameObject _arrowPrefab;

        [SerializeField]
        private GameObject _magicMissilePrefab;

        [SerializeField]
        private Transform _temporaryParent;

        [SerializeField]
        private Transform _projectileSpawn;

        [SerializeField]
        private float _projectileSpeed;

        [SerializeField]
        private float _meleeAttackDelay = 1.167f;

        [SerializeField]
        private Animator _animator;

        private float _attackCooldown = 0f;

        private PlayerStats _playerStats;

        public override void Attack(CharacterStats targetStats)
        {
            base.Attack(targetStats);

            Debug.Log("Attempt Attack");

            if (_attackCooldown <= 0f)
            {
                var attackType = EquipmentManager.Instance.CurrentWeapon.AttackType;

                Debug.Log("Attack Not On Cooldown");

                switch (attackType)
                {
                    case AbilityType.Melee:
                    {
                        Debug.Log("Melee Attack Case");

                        MeleeAttack(targetStats);

                        break;
                    }

                    case AbilityType.Ranged:
                    {
                        Debug.Log("Ranged Attack Case");

                        RangedAttack(targetStats);

                        break;
                    }

                    case AbilityType.Magic:
                    {
                        Debug.Log("Magic Attack Case");

                        MagicAttack(targetStats);

                        break;
                    }
                }

                _attackCooldown = 1f / _playerStats.AttackSpeed.GetValue();
            }
        }

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

        private void MeleeAttack(CharacterStats targetStats)
        {
            Debug.Log("Melee Attack Start");

            _animator.SetTrigger("attack");

            StartCoroutine(DoDamage(targetStats, _meleeAttackDelay));
        }

        private void RangedAttack(CharacterStats targetStats)
        {
            Debug.Log("Ranged Attack Start");

            _animator.SetTrigger("bow");

            ShootProjectile(_arrowPrefab, targetStats);
        }

        private void MagicAttack(CharacterStats targetStats)
        {
            Debug.Log("Magic Attack Start");

            ShootProjectile(_magicMissilePrefab, targetStats);
        }

        private void ShootProjectile(GameObject prefab, CharacterStats targetStats)
        {
            Debug.Log("Shoot Projectile Started");

            var clone = Instantiate(prefab, _projectileSpawn.position, Quaternion.identity, _temporaryParent).GetComponent<Rigidbody>();

            clone.GetComponent<Projectile>().Damage = _playerStats.AttackDamage.GetValue();

            var target = targetStats.gameObject;

            var velocity = (target.transform.position - transform.position).normalized * _projectileSpeed;

            clone.velocity = velocity;

            Debug.Log("Shoot Projectile Ended");
        }

        private void Test(CharacterStats targetStats)
        {
            _animator.SetTrigger("bow");

            var enemy = targetStats.gameObject;

            var arrow = Instantiate(_arrowPrefab, _projectileSpawn.position, Quaternion.identity, _temporaryParent).GetComponent<Rigidbody>();

            arrow.gameObject.GetComponent<Projectile>().Damage = _playerStats.AttackDamage.GetValue();

            var targetPosition = (enemy.transform.position - _projectileSpawn.position) / 3;

            arrow.velocity = targetPosition * _projectileSpeed;
        }
    }
}