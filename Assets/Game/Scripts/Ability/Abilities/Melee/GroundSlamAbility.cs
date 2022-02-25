using Sins.AI;
using Sins.Character;
using UnityEngine;

namespace Sins.Abilities
{
    public class GroundSlamAbility : MonoBehaviour
    {
        [SerializeField]
        private Ability _ability;

        [SerializeField]
        private int _minimumDamage = 2;

        [SerializeField]
        private int _maximumDamage = 4;

        [SerializeField]
        private int _stunChance = 25;

        [SerializeField]
        private float _radius = 10;

        [SerializeField]
        private LayerMask _enemyMask;

        [SerializeField]
        private LayerMask _groundMask;

        [SerializeField]
        private Animator _animator;

        private void Awake()
        {
            if (_ability != null)
            {
                _ability.OnAbilityUsed.AddListener(cooldown => Use());

                _ability.CanUse = true;
            }
        }

        public void Use()
        {
            _animator.SetTrigger("groundSlam");

            var colliders = Physics.OverlapSphere(transform.position, _radius, _enemyMask);

            if (colliders.Length > 0)
            {
                foreach (var collider in colliders)
                {
                    if (collider.gameObject != null)
                    {
                        var damage = Random.Range(_minimumDamage, _maximumDamage);

                        var randomValue = Random.Range(0, 100);

                        if (randomValue <= _stunChance)
                        {
                            var enemyController = collider.gameObject.GetComponent<EnemyController>();

                            if (enemyController != null)
                            {
                                enemyController.Stun();
                            }
                        }

                        var enemyStats = collider.gameObject.GetComponent<EnemyStats>();

                        if (enemyStats != null)
                        {
                            enemyStats.Damage(damage);
                        }
                    }
                }
            }
        }
    }
}