using Sins.AI;
using Sins.Character;
using UnityEngine;
using UnityEngine.VFX;

namespace Sins.Abilities
{
    public class MoltenStrikeAbility : MonoBehaviour
    {
        [SerializeField]
        private Ability _ability;

        [SerializeField]
        private int _minimumDamage = 1;

        [SerializeField]
        private int _maximumDamage = 3;

        [SerializeField]
        private int _burnChance = 25;

        [SerializeField]
        private float _radius = 10;

        [SerializeField]
        private Camera _camera;

        [SerializeField]
        private LayerMask _enemyMask;

        [SerializeField]
        private LayerMask _groundMask;

        [SerializeField]
        private Animator _animator;

        private void Awake()
        {
            _ability.OnAbilityUsed.AddListener(cooldown => Use());

            _ability.CanUse = true;
        }

        public void Use()
        {
            var ray = _camera.ScreenPointToRay(Input.mousePosition);

            var mousePosition = Vector3.zero;

            if (Physics.Raycast(ray, out var raycastHit, float.MaxValue, _groundMask))
            {
                mousePosition = raycastHit.point;
            }

            transform.LookAt(mousePosition);

            _animator.SetTrigger("attack");

            var colliders = Physics.OverlapSphere(mousePosition, _radius, _enemyMask);

            if (colliders.Length > 0)
            {
                foreach (var collider in colliders)
                {
                    if (collider.gameObject != null)
                    {
                        var damage = Random.Range(_minimumDamage, _maximumDamage);

                        var randomValue = Random.Range(0, 100);

                        if (randomValue <= _burnChance)
                        {
                            // Burn effect here

                            collider.gameObject.GetComponent<EnemyController>().Stun();
                        }

                        collider.gameObject.transform.Find("Molten_Strike_Effect").GetComponent<VisualEffect>().Play();

                        collider.gameObject.GetComponent<EnemyStats>().Damage(damage);
                    }
                }
            }
        }
    }
}