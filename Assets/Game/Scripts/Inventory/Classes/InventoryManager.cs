using System;
using System.Linq;
using UnityEngine;

namespace Sins.Inventory
{
    public class InventoryManager : IInventoryManager
    {
        private Vector2Int _size = Vector2Int.one;

        private Rect _fullRect;

        private IInventoryProvider _provider;

        public int Width => _size.x;
        public int Height => _size.y;

        public bool IsFull
        {
            get
            {
                if (_provider.IsInventoryFull) return true;

                for (int x = 0; x < Width; x++)
                {
                    for (int y = 0; y < Height; y++)
                    {
                        if (GetAtPoint(new Vector2Int(x, y)) == null)
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
        }

        public IInventoryItem[] AllItems { get; private set; }

        public Action OnRebuilt { get; set; }
        public Action OnResized { get; set; }
        public Action<IInventoryItem> OnItemDropped { get; set; }
        public Action<IInventoryItem> OnItemDroppedFailed { get; set; }
        public Action<IInventoryItem> OnItemAdded { get; set; }
        public Action<IInventoryItem> OnItemAddedFailed { get; set; }
        public Action<IInventoryItem> OnItemRemoved { get; set; }

        public InventoryManager(IInventoryProvider provider, int width, int height)
        {
            _provider = provider;

            Rebuild();
            Resize(width, height);
        }

        internal bool TryForceDrop(IInventoryItem item)
        {
            if (!item.CanDrop)
            {
                OnItemDroppedFailed?.Invoke(item);

                return false;
            }

            OnItemDropped?.Invoke(item);

            return true;
        }

        public void Resize(int newWidth, int newHeight)
        {
            _size.x = newWidth;
            _size.y = newHeight;

            RebuildRect();
        }

        public void Rebuild() => Rebuild(false);

        public void Dispose()
        {
            _provider = null;
            AllItems = null;
        }

        public IInventoryItem GetAtPoint(Vector2Int point)
        {
            if (_provider.InventoryRenderMode == InventoryRenderMode.Single && _provider.IsInventoryFull && AllItems.Length > 0)
            {
                return AllItems[0];
            }

            foreach (var item in AllItems)
            {
                if (item.Contains(point))
                {
                    return item;
                }
            }

            return null;
        }

        public IInventoryItem[] GetAtPoint(Vector2Int point, Vector2Int size)
        {
            var possibleItems = new IInventoryItem[size.x * size.y];

            var counter = 0;

            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    possibleItems[counter] = GetAtPoint(point + new Vector2Int(x, y));

                    counter++;
                }
            }

            return possibleItems.Distinct().Where(x => x != null).ToArray();
        }

        public bool TryRemove(IInventoryItem item)
        {
            if (!CanRemove(item)) return false;
            if (!_provider.RemoveInventoryItem(item)) return false;

            Rebuild(true);

            OnItemRemoved?.Invoke(item);

            return true;
        }

        public bool TryDrop(IInventoryItem item)
        {
            if (!CanDrop(item) || !_provider.DropInventoryItem(item))
            {
                OnItemDroppedFailed?.Invoke(item);

                return false;
            }

            Rebuild(true);

            OnItemDropped?.Invoke(item);

            return true;
        }

        public bool CanAddAt(IInventoryItem item, Vector2Int point)
        {
            if (!_provider.CanAddInventoryItem(item) || _provider.IsInventoryFull)
            {
                return false;
            }

            if (_provider.InventoryRenderMode == InventoryRenderMode.Single)
            {
                return true;
            }

            var previousPoint = item.Position;

            item.Position = point;

            var padding = Vector2.one * 0.01f;

            // Check if item is outside of inventory
            if (!_fullRect.Contains(item.GetMinimumPoint() + padding) || !_fullRect.Contains(item.GetMaximumPoint() - padding))
            {
                item.Position = previousPoint;

                return false;
            }

            // Check if item overlaps another item already in the inventory
            if (!AllItems.Any(otherItem => item.Overlaps(otherItem)))
            {
                return true;
            }

            item.Position = previousPoint;

            return false;
        }

        public bool TryAddAt(IInventoryItem item, Vector2Int point)
        {
            if (!CanAddAt(item, point) || !_provider.AddInventoryItem(item))
            {
                OnItemAddedFailed?.Invoke(item);

                return false;
            }

            switch (_provider.InventoryRenderMode)
            {
                case InventoryRenderMode.Single:
                {
                    item.Position = GetCentrePosition(item);

                    break;
                }

                case InventoryRenderMode.Grid:
                {
                    item.Position = point;

                    break;
                }

                default:
                {
                    throw new NotImplementedException($"InventoryRenderMode.{_provider.InventoryRenderMode} has not been implemented");
                }
            }

            Rebuild(true);

            OnItemAdded?.Invoke(item);

            return true;
        }

        public bool CanAdd(IInventoryItem item)
        {
            if (!Contains(item) && GetFirstPointThatFitsItem(item, out Vector2Int point))
            {
                return CanAddAt(item, point);
            }

            return false;
        }

        public bool TryAdd(IInventoryItem item)
        {
            if (!CanAdd(item)) return false;

            return GetFirstPointThatFitsItem(item, out Vector2Int point) && TryAddAt(item, point);
        }

        public bool CanSwap(IInventoryItem item) => _provider.InventoryRenderMode == InventoryRenderMode.Single && DoesItemFit(item) && _provider.CanAddInventoryItem(item);

        public void DropAll()
        {
            var itemsToDrop = AllItems.ToArray();

            foreach (var item in itemsToDrop)
            {
                TryDrop(item);
            }
        }

        public void Clear()
        {
            foreach (var item in AllItems)
            {
                TryRemove(item);
            }
        }

        public bool Contains(IInventoryItem item) => AllItems.Contains(item);

        public bool CanRemove(IInventoryItem item) => Contains(item) && _provider.CanRemoveInventoryItem(item);

        public bool CanDrop(IInventoryItem item) => Contains(item) && _provider.CanDropInventoryItem(item) && item.CanDrop;

        private void Rebuild(bool silent)
        {
            AllItems = new IInventoryItem[_provider.InventoryItemCount];

            for (int i = 0; i < _provider.InventoryItemCount; i++)
            {
                AllItems[i] = _provider.GetInventoryItem(i);
            }

            if (!silent)
            {
                OnRebuilt?.Invoke();
            }
        }

        private void RebuildRect()
        {
            _fullRect = new Rect(0, 0, _size.x, _size.y);

            HandleSizeChanged();

            OnResized?.Invoke();
        }

        private void HandleSizeChanged()
        {
            foreach (var item in AllItems)
            {
                var shouldBeDropped = false;
                var padding = Vector2.one * 0.01f;

                if (!_fullRect.Contains(item.GetMinimumPoint() + padding) || !_fullRect.Contains(item.GetMaximumPoint() - padding))
                {
                    shouldBeDropped = true;
                }

                if (shouldBeDropped)
                {
                    TryDrop(item);
                }
            }
        }

        // Get first free point that will fit the given item
        private bool GetFirstPointThatFitsItem(IInventoryItem item, out Vector2Int point)
        {
            if (DoesItemFit(item))
            {
                for (int x = 0; x < Width - (item.Width - 1); x++)
                {
                    for (int y = 0; y < Height - (item.Height - 1); y++)
                    {
                        point = new Vector2Int(x, y);

                        if (CanAddAt(item, point))
                        {
                            return true;
                        }
                    }
                }
            }

            point = Vector2Int.zero;

            return false;
        }

        // Returns true if give items fits within this inventory
        private bool DoesItemFit(IInventoryItem item) => item.Width <= Width && item.Height <= Height;

        // Returns the centre postition for a given item within this inventory
        private Vector2Int GetCentrePosition(IInventoryItem item) => new Vector2Int(_size.x - item.Width / 2, _size.y - item.Height / 2);
    }
}