using UnityEngine;

namespace Sins.Abilities
{
    public class DeadShotAbility : MonoBehaviour
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
        private int _throwForce = 100;

        [SerializeField]
        private int _minimumDamage = 5;

        [SerializeField]
        private int _maximumDamage = 10;

        [SerializeField]
        private float _radius = 2f;

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
                        var enemy = collider.gameObject;

                        var head = enemy.transform.Find("Head");

                        var arrow = Instantiate(_arrowPrefab, _projectileSpawn.position, Quaternion.identity, _temporaryParent).GetComponent<Rigidbody>();

                        var damage = Random.Range(_minimumDamage, _maximumDamage);

                        arrow.gameObject.GetComponent<Projectile>().Damage = damage;

                        var targetPosition = (head.position - _projectileSpawn.position) / 3;

                        arrow.velocity = targetPosition * _throwForce;
                    }
                }
            }
        }
    }
}