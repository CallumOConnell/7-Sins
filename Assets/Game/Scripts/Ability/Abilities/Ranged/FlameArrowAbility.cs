using Sins.Utils;
using UnityEngine;

namespace Sins.Abilities
{
    public class FlameArrowAbility : MonoBehaviour
    {
        [SerializeField]
        private Ability _ability;

        [SerializeField]
        private GameObject _arrowPrefab;

        [SerializeField]
        private Transform _projectileSpawn;

        [SerializeField]
        private Transform _temporaryParent;

        [SerializeField]
        private int _projectileSpeed;

        [SerializeField]
        private int _minimumDamage = 1;

        [SerializeField]
        private int _maximumDamage = 2;

        [SerializeField]
        private float _radius = 5f;

        [SerializeField]
        private Camera _camera;

        [SerializeField]
        private LayerMask _enemyMask;

        [SerializeField]
        private LayerMask _groundMask;

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

            var colliders = Physics.OverlapSphere(mousePosition, _radius, _enemyMask);

            if (colliders.Length > 0)
            {
                foreach (var collider in colliders)
                {
                    if (collider != null)
                    {
                        var target = collider.gameObject;

                        transform.LookAt(target.transform);

                        var arrow = Instantiate(_arrowPrefab, _projectileSpawn.position, Quaternion.identity, _temporaryParent).GetComponent<Rigidbody>();

                        var damage = Random.Range(_minimumDamage, _maximumDamage);

                        arrow.gameObject.GetComponent<Projectile>().Damage = damage;

                        var velocity = (target.transform.position - transform.position).normalized * _projectileSpeed;

                        arrow.velocity = velocity;
                    }
                }
            }
        }
    }
}