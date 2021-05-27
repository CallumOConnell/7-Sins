using System;
using UnityEngine;

namespace Sins.Inventory
{
    public interface IInventoryManager : IDisposable
    {
        Action<IInventoryItem> OnItemAdded { get; set; }
        Action<IInventoryItem> OnItemAddedFailed { get; set; }
        Action<IInventoryItem> OnItemRemoved { get; set; }
        Action<IInventoryItem> OnItemDropped { get; set; }
        Action<IInventoryItem> OnItemDroppedFailed { get; set; }
        Action OnRebuilt { get; set; }
        Action OnResized { get; set; }

        IInventoryItem[] AllItems { get; } // Returns all items in the inventory

        int Width { get; }
        int Height { get; }

        bool IsFull { get; } // Returns true if the inventory is full

        bool Contains(IInventoryItem item); // Returns true if the given item is present in the inventory
        bool CanAdd(IInventoryItem item);
        bool TryAdd(IInventoryItem item);
        bool CanAddAt(IInventoryItem item, Vector2Int point);
        bool TryAddAt(IInventoryItem item, Vector2Int point);
        bool CanRemove(IInventoryItem item);
        bool CanSwap(IInventoryItem item);
        bool TryRemove(IInventoryItem item);
        bool CanDrop(IInventoryItem item);
        bool TryDrop(IInventoryItem item);

        void DropAll();
        void Clear();
        void Rebuild();
        void Resize(int width, int height); // Sets the width and height of the inventory

        IInventoryItem GetAtPoint(Vector2Int point);
        IInventoryItem[] GetAtPoint(Vector2Int point, Vector2Int size);
    }
}