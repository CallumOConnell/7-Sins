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
            Debug.Log("A");
            if (other.gameObject != null)
            {
                Debug.Log("B");
                if (other.gameObject.CompareTag("Projectile"))
                {
                    Debug.Log("C");
                    var characterStats = GetComponent<CharacterStats>();

                    if (characterStats != null)
                    {
                        Debug.Log("D");
                        var projectile = other.GetComponent<Projectile>();

                        if (projectile != null)
                        {
                            Debug.Log("E");
                            var impactEffect = projectile.gameObject.transform.GetChild(0).GetComponent<VisualEffect>();

                            Debug.Log($"{impactEffect.name}");

                            if (projectile.UseImpactEffect && impactEffect != null)
                            {
                                Debug.Log("F");
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