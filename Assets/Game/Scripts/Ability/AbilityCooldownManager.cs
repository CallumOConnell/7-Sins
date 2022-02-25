using System.Collections;
using UnityEngine;

namespace Sins.Abilities
{
    public class AbilityCooldownManager : MonoBehaviour
    {
        public static AbilityCooldownManager Instance;

        private void Awake() => Instance = this;

        public void StartCooldown(Ability ability)
        {
            StartCoroutine(Cooldown());

            IEnumerator Cooldown()
            {
                Debug.Log($"Cooldown for {ability.Title} started");

                ability.CanUse = false;

                yield return new WaitForSeconds(ability.CooldownTime);

                ability.CanUse = true;

                Debug.Log($"Cooldown for {ability.Title} ended");
            }
        }
    }
}