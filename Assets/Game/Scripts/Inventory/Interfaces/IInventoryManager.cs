using System;
using UnityEngine;

namespace Endure.Inventory
{
    public interface IInventoryManager : IDisposable
    {
        Action<IInventoryItem> OnItemAdded { get; set; }
        Action<IInventoryItem> OnItemAddedFailed { get; set; }
        Action<IInventoryItem> OnItemRemoved { get; set; }
        Action<IInventoryItem> OnItemDropped { get; set; }
        Action<IInventoryItem> OnItemDroppedFailed { get; set; }
        Action OnRebuild { get; set; }
        Action OnResized { get; set; }

        IInventoryItem[] Items { get; } // Returns all items in the inventory

        int Width { get; }
        int Height { get; }

        bool IsFull { get; } // Returns true if the inventory is full

        bool InventoryContainsItem(IInventoryItem item); // Returns true if the given item is present in the inventory
        bool CanAddItemToInventory(IInventoryItem item);
        bool TryAddItemToInventory(IInventoryItem item);
        bool CanAddItemAtPoint(IInventoryItem item, Vector2Int point);
        bool TryAddItemAtPoint(IInventoryItem item, Vector2Int point);
        bool CanRemoveItemFromInventory(IInventoryItem item);
        bool CanSwapItem(IInventoryItem item);
        bool TryRemoveItem(IInventoryItem item);
        bool CanDropItem(IInventoryItem item);

        void DropAllItemsFromInventory();
        void ClearInventory();
        void RebuildInventory();
        void ResizeInventory(int width, int height); // Sets the width and height of the inventory

        IInventoryItem GetItemAtPoint(Vector2Int point);
        IInventoryItem[] GetItemsAtPoint(Vector2Int point, Vector2Int size);
    }
}