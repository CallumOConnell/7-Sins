using UnityEngine;

namespace Sins.Abilities
{
    public class LureAbility : MonoBehaviour
    {
        [SerializeField]
        private Ability _ability;

        [SerializeField]
        private GameObject _lurePrefab;

        [SerializeField]
        private Transform _temporaryParent;

        [SerializeField]
        private Transform _projectileSpawn;

        [SerializeField]
        private Transform _player;

        [SerializeField]
        private Camera _camera;

        [SerializeField]
        private int _throwForce;

        [SerializeField]
        private LayerMask _groundMask;

        private void Awake()
        {
            _ability.OnAbilityUsed.AddListener(cooldown => Use());

            _ability.CanUse = true;
        }

        public void Use()
        {
            var lureObject = Instantiate(_lurePrefab, _projectileSpawn.position, Quaternion.identity, _temporaryParent).GetComponent<Rigidbody>();

            var ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var raycastHit, float.MaxValue, _groundMask))
            {
                var targetPosition = (raycastHit.point - _projectileSpawn.position) / 3;

                lureObject.velocity = targetPosition * _throwForce;

                lureObject.GetComponent<Bait>().StartDistraction();
            }
        }
    }
}