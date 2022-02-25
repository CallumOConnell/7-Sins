using Sins.Abilities;
using Sins.Character;
using System.Collections;
using UnityEngine;

namespace Sins.AI
{
    [RequireComponent(typeof(CharacterStats))]
    public class EnemyCombat : CharacterCombat
    {
        [SerializeField]
        private Transform _temporaryParent;

        [SerializeField]
        private GameObject _magicProjectilePrefab;

        [SerializeField]
        private GameObject _rangedProjectilePrefab;

        [SerializeField]
        private float _projectileSpeed = 1f;

        [SerializeField]
        private Transform _projectileSpawnPoint;

        [SerializeField]
        private float _attackDelay = 0.6f;

        [SerializeField]
        private Animator _animator;

        private EnemyStats _enemyStats;

        private float _attackCooldown = 0f;

        private void Start() => _enemyStats = GetComponent<EnemyStats>();

        private void Update() => _attackCooldown -= Time.deltaTime;

        private void ShootProjectile(GameObject prefab)
        {
            var clone = Instantiate(prefab, _projectileSpawnPoint.position, Quaternion.identity, _temporaryParent).GetComponent<Rigidbody>();

            clone.GetComponent<Projectile>().Damage = _enemyStats.AttackDamage.GetValue();

            var target = Player.Instance;

            var velocity = (target.transform.position - transform.position).normalized * _projectileSpeed;

            clone.velocity = velocity;
        }

        private IEnumerator DoDamage(CharacterStats targetStats, float delay)
        {
            yield return new WaitForSeconds(delay);

            targetStats.Damage(_enemyStats.AttackDamage.GetValue());
        }

        public override void Attack(CharacterStats targetStats)
        {
            base.Attack(targetStats);

            if (_attackCooldown <= 0)
            {
                switch (_enemyStats.Type)
                {
                    case EnemyType.Melee:
                    {
                        _animator.SetTrigger("meleeAttack");

                        StartCoroutine(DoDamage(targetStats, _attackDelay));

                        break;
                    }

                    case EnemyType.Ranged:
                    {
                        _animator.SetTrigger("rangedAttack");

                        ShootProjectile(_rangedProjectilePrefab);

                        break;
                    }

                    case EnemyType.Magic:
                    {
                        _animator.SetTrigger("magicAttack");

                        ShootProjectile(_magicProjectilePrefab);

                        break;
                    }
                }

                _attackCooldown = 1f / _enemyStats.AttackSpeed.GetValue();
            }
        }
    }
}