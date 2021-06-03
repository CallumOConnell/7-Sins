using UnityEngine;

namespace Sins.Abilities
{
    public class FireballAbility : MonoBehaviour
    {
        [SerializeField]
        private Ability _ability;

        [SerializeField]
        private GameObject _fireballPrefab;

        [SerializeField]
        private Transform _projectileSpawn;

        [SerializeField]
        private Transform _temporaryParent;

        [SerializeField]
        private int _projectileSpeed;

        [SerializeField]
        private int _minimumDamage = 2;

        [SerializeField]
        private int _maximumDamage = 4;

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

                        var fireball = Instantiate(_fireballPrefab, _projectileSpawn.position, Quaternion.identity, _temporaryParent).GetComponent<Rigidbody>();

                        var damage = Random.Range(_minimumDamage, _maximumDamage);

                        fireball.gameObject.GetComponent<Projectile>().Damage = damage;

                        var velocity = (target.transform.position - transform.position).normalized * _projectileSpeed;

                        fireball.velocity = velocity;
                    }
                }
            }
        }
    }
}