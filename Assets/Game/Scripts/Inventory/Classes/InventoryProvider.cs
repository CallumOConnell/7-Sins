using System.Collections.Generic;

namespace Sins.Inventory
{
    public class InventoryProvider : IInventoryProvider
    {
        private List<IInventoryItem> _items = new List<IInventoryItem>();

        private int _maximumAllowedItemCount;

        private ItemType _allowedItem;

        public int InventoryItemCount => _items.Count;

        public InventoryRenderMode InventoryRenderMode { get; private set; }

        public InventoryProvider(InventoryRenderMode renderMode,  int maximumAllowedItemCount = -1, ItemType allowedItem = ItemType.Any)
        {
            InventoryRenderMode = renderMode;
            _maximumAllowedItemCount = maximumAllowedItemCount;
            _allowedItem = allowedItem;
        }

        public bool IsInventoryFull
        {
            get
            {
                if (_maximumAllowedItemCount < 0) return false;

                return InventoryItemCount >= _maximumAllowedItemCount;
            }
        }

        public bool AddInventoryItem(IInventoryItem item)
        {
            if (!_items.Contains(item))
            {
                _items.Add(item);

                return true;
            }

            return false;
        }

        public bool DropInventoryItem(IInventoryItem item) => RemoveInventoryItem(item);

        public IInventoryItem GetInventoryItem(int index) => _items[index];

        public bool CanAddInventoryItem(IInventoryItem item)
        {
            if (_allowedItem == ItemType.Any) return true;

            return (item as ItemDefinition).Type == _allowedItem;
        }

        public bool CanRemoveInventoryItem(IInventoryItem item) => true;

        public bool CanDropInventoryItem(IInventoryItem item) => true;

        public bool RemoveInventoryItem(IInventoryItem item) => _items.Remove(item);
    }
}