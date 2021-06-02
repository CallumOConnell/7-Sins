using System.Collections.Generic;
using UnityEngine;
using Sins.Inventory;
using Sins.Interaction;
using System;
using UnityEngine.VFX;

namespace Sins.Loot
{
    public class LootChest : MonoBehaviour
    {
        [SerializeField]
        private int _minimumAmountToDrop = 1;

        [SerializeField]
        private int _maximumAmountToDrop = 4;

        [SerializeField]
        private int _spawnRadius = 1;

        [SerializeField]
        private LootTable _lootTable;

        [SerializeField]
        private LayerMask _groundLayer;

        [Header("Rarity Colours"), Space]

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

        private bool _looted = false;

        private List<ItemDefinition> _itemsToDrop = new List<ItemDefinition>();

        // This function will be called from an interaction script
        public void SpawnLoot()
        {
            if (!_looted)
            {
                // Calculate how many items to drop from the chest
                var amountOfItemsToDrop = UnityEngine.Random.Range(_minimumAmountToDrop, _maximumAmountToDrop);

                // Calculate the items that will be dropped (populates _itemsToDrop list)
                CalculateDropItems(amountOfItemsToDrop);

                // Spawn the items in the world around the chest
                if (_itemsToDrop.Count > 0)
                {
                    foreach (var item in _itemsToDrop)
                    {
                        ItemDefinition newItem = null;

                        if (item is Equipment)
                        {
                            newItem = CalculateItemStats(item);
                        }

                        if (newItem != null)
                        {
                            var itemPosition = CalculatePosition();

                            var lootItem = Instantiate(newItem.Prefab, transform.position - itemPosition, Quaternion.identity);

                            lootItem.GetComponent<PickupItem>().Item = newItem;

                            SetItemVisualEffect(lootItem, newItem.Rarity);

                            _looted = true;
                        }
                    }
                }
            }
        }

        private void CalculateDropItems(int amountToDrop)
        {
            if (_lootTable != null)
            {
                _itemsToDrop.Clear();

                for (var i = 0; i < amountToDrop; i++)
                {
                    _itemsToDrop.Add(_lootTable.SelectLootItem());
                }
            }
        }

        private Vector3 CalculatePosition()
        {
            var angle = UnityEngine.Random.Range(0, Mathf.PI * 2);

            var itemPosition = new Vector3(Mathf.Sin(angle), -0.05f, Mathf.Cos(angle));

            itemPosition *= _spawnRadius;

            return itemPosition;
        }

        private float GetGroundHeight(Vector3 position)
        {
            position += new Vector3(0, 100, 0);

            if (Physics.Raycast(position, Vector3.down, out RaycastHit hit, 1000, _groundLayer))
            {
                return hit.point.y;
            }

            return Mathf.NegativeInfinity;
        }

        private ItemDefinition CalculateItemStats(ItemDefinition item)
        {
            // Select item rarity
            var values = Enum.GetValues(typeof(ItemRarity));

            var rarity = UnityEngine.Random.Range(0, values.Length);

            item.Rarity = (ItemRarity)rarity;

            // Check if item is weapon or armour
            if (item is Weapon)
            {
                var weapon = item as Weapon;

                weapon.AttackDamageModifier = GenerateStat(item.Rarity);
                weapon.AttackSpeedModifier = GenerateStat(item.Rarity);
            }
            else
            {
                var armour = item as Armour;

                armour.MaxHealthModifier = GenerateStat(item.Rarity);
                armour.ArmourModifier = GenerateStat(item.Rarity);
                //armour.MovementSpeedModifier = GenerateStat(item.Rarity);
            }

            return item;
        }

        private int GenerateStat(ItemRarity rarity)
        {
            switch (rarity)
            {
                case ItemRarity.Common:
                {
                    return UnityEngine.Random.Range(0, 20);
                }
                
                case ItemRarity.Uncommon:
                {
                    return UnityEngine.Random.Range(21, 50);
                }

                case ItemRarity.Rare:
                {
                    return UnityEngine.Random.Range(51, 100);
                }

                case ItemRarity.Epic:
                {
                    return UnityEngine.Random.Range(101, 150);
                }

                case ItemRarity.Legendary:
                {
                    return UnityEngine.Random.Range(151, 200);
                }

                default:
                {
                    return 0;
                }
            }
        }

        private void SetItemVisualEffect(GameObject lootItem, ItemRarity rarity)
        {
            var itemVFX = lootItem.transform.GetChild(0).GetComponent<VisualEffect>();

            if (itemVFX != null)
            {
                switch (rarity)
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