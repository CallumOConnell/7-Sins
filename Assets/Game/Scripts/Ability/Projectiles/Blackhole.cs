using Sins.Character;
using UnityEngine;

namespace Sins.Abilities
{
    public class Blackhole : MonoBehaviour
    {
        [SerializeField]
        private float _lifeTime = 20f;

        [SerializeField]
        private float _gravityPull = 0.78f;

        public int DamageDealt { private get; set; }

        private void Update()
        {
            _lifeTime -= Time.deltaTime;

            if (_lifeTime <= 0)
            {
                Destroy(gameObject);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject != null)
            {
                if (other.attachedRigidbody)
                {
                    if (other.CompareTag("Enemy"))
                    {
                        // Pull enemy towards the hole
                        other.transform.position = Vector3.MoveTowards(other.transform.position, transform.position, _gravityPull * Time.deltaTime);

                        var enemyStats = other.gameObject.GetComponent<EnemyStats>();

                        if (enemyStats != null)
                        {
                            enemyStats.Damage(DamageDealt);
                        }
                    }
                }
            }
        }
    }
}