using UnityEngine;

namespace Sins.Inventory
{
    public interface IInventoryItem
    {
        Sprite Sprite { get; } // Current sprite of the item

        Vector2Int Position { get; set; } // Current position of the item in the inventory

        int Width { get; }
        int Height { get; }

        bool CanDrop { get; } // Returns true if this item can be dropped on the ground

        bool IsPartOfShape(Vector2Int localPosition); // Returns true if given local position is part of this items shape
    }

    internal static class InventoryItemExtensions
    {
        // Returns the lower left corner position of an item
        internal static Vector2Int GetMinimumPoint(this IInventoryItem item) => item.Position;

        // Returns the top right corner position of an item
        internal static Vector2Int GetMaximumPoint(this IInventoryItem item) => item.Position + new Vector2Int(item.Width, item.Height);

        // Returns true if the item overlaps the given point within an inventory
        internal static bool Contains(this IInventoryItem item, Vector2Int inventoryPoint)
        {
            for (int x = 0; x < item.Width; x++)
            {
                for (int y = 0; y < item.Height; y++)
                {
                    var iPoint = item.Position + new Vector2Int(x, y);

                    if (iPoint == inventoryPoint) return true;
                }
            }

            return false;
        }

        // Returns true if item overlaps given item
        internal static bool Overlaps(this IInventoryItem item, IInventoryItem otherItem)
        {
            for (var x = 0; x < item.Width; x++)
            {
                for (var y = 0; y < item.Height; y++)
                {
                    if (item.IsPartOfShape(new Vector2Int(x, y)))
                    {
                        var itemPoint = item.Position + new Vector2Int(x, y);

                        for (var x1 = 0; x1 < otherItem.Width; x1++)
                        {
                            for (var y1 = 0; y1 < otherItem.Height; y1++)
                            {
                                if (otherItem.IsPartOfShape(new Vector2Int(x1, y1)))
                                {
                                    var otherItemPoint = otherItem.Position + new Vector2Int(x1, y1);

                                    if (otherItemPoint == itemPoint) return true;
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }
    }
}