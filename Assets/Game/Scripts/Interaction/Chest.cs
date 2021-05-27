using Sins.Loot;
using System.Collections;
using UnityEngine;

namespace Sins.Interaction
{
    public class Chest : Interactable
    {
        public override void Interact()
        {
            base.Interact();

            StartCoroutine(OpenChest());
        }

        private IEnumerator OpenChest()
        {
            GetComponent<Animator>().SetBool("Open", true);

            yield return new WaitForSeconds(1.067f);

            GetComponent<LootChest>().SpawnLoot();
        }
    }
}