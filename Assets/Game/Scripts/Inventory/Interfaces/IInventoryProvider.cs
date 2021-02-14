namespace Endure.Inventory
{
    public interface IInventoryProvider
    {
        // Returns the total amount of inventory items in the current inventory.
        int InventoryItemCount { get; }

        // Returns true if the inventory is full.
        bool IsInventoryFull { get; }

        // Returns inventory item at given index.
        IInventoryItem GetInventoryItem(int index);

        // Returns true if given inventory item is allowed inside this inventory
        bool CanAddInventoryItem(IInventoryItem item);

        // Returns true if the given inventory item is allowed to be removed from the inventory.
        bool CanRemoveInventoryItem(IInventoryItem item);

        // Returns true if the given inventory item is allowed to be dropped on the ground.
        bool CanDropInventoryItem(IInventoryItem item);

        // Invoked when an inventory item is added to the inventory. Returns true if successful.
        bool AddInventoryItem(IInventoryItem item);

        // Invoked when an inventory item is removed from the inventory. Returns true if successful.
        bool RemoveInventoryItem(IInventoryItem item);

        // Invoked when an inventory item is removed from the inventory and should be placed onto the ground. Returns true if successful.
        bool DropInventoryItem(IInventoryItem item);
    }
}