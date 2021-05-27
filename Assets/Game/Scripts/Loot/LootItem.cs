using Sins.Inventory;

namespace Sins.Loot
{
    [System.Serializable]
    public class LootItem
    {
        public string ItemName;

        public float DropChance;

        public ItemDefinition ItemObject;
    }
}