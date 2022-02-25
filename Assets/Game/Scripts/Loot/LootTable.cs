using UnityEngine;
using Sins.Inventory;
using System.Linq;
using System.Collections.Generic;

namespace Sins.Loot
{
    [CreateAssetMenu(fileName = "LootTable", menuName = "Loot Table")]
    public class LootTable : ScriptableObject
    {
        [SerializeField]
        private List<LootItem> _lootItems;

        private float _totalWeight;

        private void Initialise() => _totalWeight = _lootItems.Sum(item => item.DropChance);

        public ItemDefinition SelectLootItem()
        {
            Initialise();

            var diceRoll = Random.Range(0f, _totalWeight);

            foreach (var item in _lootItems)
            {
                if (item.DropChance >= diceRoll)
                {
                    return item.ItemObject;
                }

                diceRoll -= item.DropChance;
            }

            throw new System.Exception("Loot table failed to select an item");
        }
    }
}