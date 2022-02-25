using Sins.Abilities;
using Sins.Character;
using UnityEngine;
using UnityEngine.VFX;

namespace Sins.AI
{
    public class EnemyCollision : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject != null)
            {
                if (other.gameObject.CompareTag("Projectile"))
                {
                    var characterStats = GetComponent<CharacterStats>();

                    if (characterStats != null)
                    {
                        var projectile = other.GetComponent<Projectile>();

                        if (projectile != null)
                        {
                            var impactEffect = projectile.gameObject.transform.GetChild(0).GetComponent<VisualEffect>();

                            if (projectile.UseImpactEffect && impactEffect != null)
                            {
                                impactEffect.Play();
                            }

                            characterStats.Damage(projectile.Damage);

                            Destroy(projectile);
                        }
                    }
                }
            }
        }
    }
}