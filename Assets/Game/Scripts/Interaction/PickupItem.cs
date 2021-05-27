using UnityEngine;
using Sins.Inventory;
using Sins.Character;

namespace Sins.Interaction
{
    public class PickupItem : Interactable
    {
        public ItemDefinition Item;

        public override void Interact()
        {
            base.Interact();

            var player = Player.Instance;

            if (player != null)
            {
                if (player.Inventory != null)
                {
                    if (player.Inventory.Manager.TryAdd(Item.CreateInstance()))
                    {
                        Destroy(gameObject);
                    }
                    else
                    {
                        Debug.LogWarning($"Failed to add {Item.name} item");
                    }
                }
                else
                {
                    Debug.LogError("Inventory manager is null");
                }
            }
            else
            {
                Debug.LogError($"Player instance on {gameObject.name} for PickupItem script is null!");
            }
        }
    }
}