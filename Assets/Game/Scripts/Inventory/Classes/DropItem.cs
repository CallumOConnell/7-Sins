using UnityEngine;
using Sins.Interaction;
using Sins.Character;
using UnityEngine.VFX;

namespace Sins.Inventory
{
    public class DropItem : MonoBehaviour
    {
        [SerializeField]
        private int _dropDistance;

        [SerializeField, ColorUsage(true, true)]
        private Color _commonColour;

        [SerializeField, ColorUsage(true, true)]
        private Color _uncommonColour;

        [SerializeField, ColorUsage(true, true)]
        private Color _rareColour;

        [SerializeField, ColorUsage(true, true)]
        private Color _epicColour;

        [SerializeField, ColorUsage(true, true)]
        private Color _legendaryColour;

        public void Dropped(ItemDefinition item)
        {
            // Get a random direction between 0 and 360 degrees
            var angle = Random.Range(0, Mathf.PI * 2);

            // Create a vector in the direction calculated above
            var itemPosition = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle));

            // Set the distance away from the player the item will be dropped
            itemPosition *= _dropDistance;

            // Create the item in the world
            var droppedItem = Instantiate(item.Prefab, Player.Instance.transform.position - itemPosition, Quaternion.identity);

            // Set the item to be picked up
            droppedItem.GetComponent<PickupItem>().Item = item;

            var itemVFX = droppedItem.transform.GetChild(0).GetComponent<VisualEffect>();

            if (itemVFX != null)
            {
                switch (item.Rarity)
                {
                    case ItemRarity.Common:
                    {
                        itemVFX.SetVector4("Colour_1", _commonColour);

                        break;
                    }
                    case ItemRarity.Uncommon:
                    {
                        itemVFX.SetVector4("Colour_1", _uncommonColour);

                        break;
                    }
                    case ItemRarity.Rare:
                    {
                        itemVFX.SetVector4("Colour_1", _rareColour);
                        itemVFX.SetVector4("Colour_2", _rareColour);

                        break;
                    }
                    case ItemRarity.Epic:
                    {
                        itemVFX.SetVector4("Colour_1", _epicColour);
                        itemVFX.SetVector4("Colour_2", _epicColour);

                        break;
                    }
                    case ItemRarity.Legendary:
                    {
                        itemVFX.SetVector4("Colour_1", _legendaryColour);
                        itemVFX.SetVector4("Colour_2", _legendaryColour);

                        break;
                    }
                }
            }
        }
    }
}