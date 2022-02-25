using Sins.AI;
using System.Collections;
using UnityEngine;

namespace Sins.Abilities
{
    public class Bait : MonoBehaviour
    {
        [SerializeField]
        private float _lifeTime = 5f;

        [SerializeField]
        private float _distractionRadius = 15f;

        [SerializeField]
        private LayerMask _enemyMask;

        public void StartDistraction()
        {
            StartCoroutine(NearbyEnemyCheck());
        }

        private IEnumerator NearbyEnemyCheck()
        {
            var colliders = Physics.OverlapSphere(transform.position, _distractionRadius, _enemyMask);

            if (colliders.Length > 0)
            {
                foreach (var collider in colliders)
                {
                    if (collider.gameObject != null)
                    {
                        var enemyController = collider.gameObject.GetComponent<EnemyController>();

                        if (enemyController != null)
                        {
                            enemyController.Distract(gameObject);
                        }
                    }
                }
            }

            yield return new WaitForSeconds(2f);
        }

        // Debug for seeing the baits detection radius
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _distractionRadius);
        }

        private void Update()
        {
            _lifeTime -= Time.deltaTime;

            if (_lifeTime <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}