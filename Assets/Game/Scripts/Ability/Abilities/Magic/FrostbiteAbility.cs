using Sins.AI;
using Sins.Character;
using UnityEngine;
using UnityEngine.VFX;

namespace Sins.Abilities
{
    public class FrostbiteAbility : MonoBehaviour
    {
        [SerializeField]
        private Ability _ability;

        [SerializeField]
        private float _range = 5f;

        [SerializeField]
        private int _minimumDamage = 2;

        [SerializeField]
        private int _maximumDamage = 4;

        [SerializeField]
        private int _freezeChance = 25;

        [SerializeField]
        private LayerMask _enemyMask;

        [SerializeField]
        private Animator _animator;

        [SerializeField]
        private VisualEffect _vfx;

        private void Awake()
        {
            _ability.OnAbilityUsed.AddListener(cooldown => Use());

            _ability.CanUse = true;
        }

        public void Use()
        {
            var offSet = 4f; // This makes the cone cast position slightly further back to avoid missing enemies directly in front of you

            var coneAngle = 0.75f;

            _animator.SetTrigger("frostbite");

            _vfx.Play();

            var colliders = Physics.OverlapSphere(transform.position, _range, _enemyMask);

            if (colliders.Length > 0)
            {
                foreach (var collider in colliders)
                {
                    if (collider != null)
                    {
                        var test = collider.transform.position - (transform.position + (-transform.forward * offSet)).normalized;

                        var dotProduct = Vector3.Dot(test, transform.forward);

                        if (dotProduct > coneAngle)
                        {
                            var damage = Random.Range(_minimumDamage, _maximumDamage);

                            var randomValue = Random.Range(0, 100);

                            if (randomValue <= _freezeChance)
                            {
                                collider.gameObject.GetComponent<EnemyController>().Stun();
                            }

                            collider.gameObject.GetComponent<EnemyStats>().Damage(damage);
                        }
                    }
                }
            }
        }
    }
}