using UnityEngine;

namespace Sins.Abilities
{
    public class BlackholeAbility : MonoBehaviour
    {
        [SerializeField]
        private Ability _ability;

        [SerializeField]
        private GameObject _blackHolePrefab;

        [SerializeField]
        private Transform _projectileSpawn;

        [SerializeField]
        private Transform _temporaryParent;

        [SerializeField]
        private int _throwForce = 100;

        [SerializeField]
        private int _minimumDamage = 5;

        [SerializeField]
        private int _maximumDamage = 8;

        [SerializeField]
        private Camera _camera;

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

            _animator.SetTrigger("blackhole");

            var blackhole = Instantiate(_blackHolePrefab, _projectileSpawn.position, Quaternion.identity, _temporaryParent).GetComponent<Rigidbody>();

            var damage = Random.Range(_minimumDamage, _maximumDamage);

            blackhole.gameObject.GetComponent<Blackhole>().DamageDealt = damage;

            var targetPosition = (mousePosition - _projectileSpawn.position) / 3;

            blackhole.velocity = targetPosition * _throwForce;
        }
    }
}