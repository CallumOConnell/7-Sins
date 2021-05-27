using Sins.Character;
using UnityEngine;
using UnityEngine.VFX;

namespace Sins.Abilities
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField]
        private float _lifeTime = 5f;

        [SerializeField]
        private bool _useImpactEffect = false;

        public int Damage { private get; set; }

        private void Update()
        {
            _lifeTime -= Time.deltaTime;

            if (_lifeTime <= 0)
            {
                Destroy(gameObject);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject != null)
            {
                if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Player"))
                {
                    var characterStats = collision.gameObject.GetComponent<CharacterStats>();

                    if (characterStats != null)
                    {
                        var impactEffect = transform.GetChild(0).GetComponent<VisualEffect>();

                        if (_useImpactEffect && impactEffect != null)
                        {
                            impactEffect.Play();
                        }

                        characterStats.Damage(Damage);
                    }
                }
            }

            Destroy(gameObject);
        }
    }
}