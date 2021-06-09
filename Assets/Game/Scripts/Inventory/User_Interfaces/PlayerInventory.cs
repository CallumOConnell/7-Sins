using UnityEngine;

namespace Sins.Inventory
{
    public class PlayerInventory : MonoBehaviour
    {
        [SerializeField]
        private InventoryRenderMode _renderMode = InventoryRenderMode.Grid;

        [SerializeField]
        private int _maximumAlowedItemCount = -1;

        [SerializeField]
        private ItemType _allowedItem = ItemType.Any;

        [SerializeField]
        private int _width = 8;

        [SerializeField]
        private int _height = 4;

        [SerializeField]
        private DropItem _dropItem;

        public InventoryManager Manager { get; private set; }

        private void Start()
        {
            var provider = new InventoryProvider(_renderMode, _maximumAlowedItemCount, _allowedItem);

            // Create inventory
            var inventory = new InventoryManager(provider, _width, _height);

            Manager = inventory;

            // Render inventory
            GetComponent<InventoryRenderer>().SetInventory(inventory, provider.InventoryRenderMode);

            inventory.OnItemDropped += (item) => _dropItem.Dropped(item as ItemDefinition);
        }
    }
}