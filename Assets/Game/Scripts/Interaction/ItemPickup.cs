using UnityEngine;

namespace Sins.Player
{
    public class ItemPickup : Interactable
    {
        


        public override void Interact()
        {
            base.Interact();

            // Try add item to inventory

            Destroy(gameObject); // Remove item from world
        }
    }
}