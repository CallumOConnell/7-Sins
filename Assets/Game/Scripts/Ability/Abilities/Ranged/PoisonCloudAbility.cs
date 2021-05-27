using Sins.Character;
using System.Collections;
using UnityEngine;

namespace Sins.Abilities
{
    public class PoisonCloudAbility : MonoBehaviour
    {
        [SerializeField]
        private Ability _ability;

        [SerializeField]
        private GameObject _poisonCloudPrefab;

        [SerializeField]
        private Transform _temporaryParent;

        [SerializeField]
        private int _minimumDamage = 1;

        [SerializeField]
        private int _maximumDamage = 3;

        [SerializeField]
        private float _effectDuration = 10f;

        [SerializeField]
        private float _radius = 10f;

        [SerializeField]
        private Camera _camera;

        [SerializeField]
        private LayerMask _enemyMask;

        [SerializeField]
        private Animator _animator;

        private bool _effectActive = false;

        private void Awake()
        {
            _ability.OnAbilityUsed.AddListener(cooldown => Use());

            _ability.CanUse = true;
        }

        private IEnumerator StartDuration()
        {
            yield return new WaitForSeconds(_effectDuration);

            _effectActive = false;
        }

        private IEnumerator Monitor(GameObject poisonCloud)
        {
            while (_effectActive)
            {
                var colliders = Physics.OverlapSphere(poisonCloud.transform.position, _radius, _enemyMask);

                if (colliders.Length > 0)
                {
                    foreach (var collider in colliders)
                    {
                        if (collider != null)
                        {
                            var enemyStats = collider.gameObject.GetComponent<EnemyStats>();

                            if (enemyStats != null)
                            {
                                var damage = Random.Range(_minimumDamage, _maximumDamage);

                                enemyStats.Damage(damage);
                            }
                        }
                    }
                }

                yield return new WaitForSeconds(1f);
            }
        }

        public void Use()
        {
            var ray = _camera.ScreenPointToRay(Input.mousePosition);

            var mousePosition = Vector3.zero;

            if (Physics.Raycast(ray, out var hit))
            {
                mousePosition = hit.point;
            }

            mousePosition.y += 2;

            _animator.SetTrigger("throw");

            var poisonCloud = Instantiate(_poisonCloudPrefab, mousePosition, Quaternion.identity, _temporaryParent);

            poisonCloud.GetComponent<ParticleSystem>().Play();

            _effectActive = true;

            StartCoroutine(Monitor(poisonCloud));

            StartCoroutine(StartDuration());
        }
    }
}