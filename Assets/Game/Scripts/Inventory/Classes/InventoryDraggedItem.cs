using UnityEngine;
using UnityEngine.UI;

namespace Sins.Inventory
{
    public class InventoryDraggedItem
    {
        public enum DropMode { Added, Swapped, Returned, Dropped }

        // Returns the InventoryController this item originated from
        public InventoryController OriginalController { get; private set; }

        // Returns the point inside the inventory from which this item originated from
        public Vector2Int OriginalPoint { get; private set; }

        // Returns the item-instance that is being dragged
        public IInventoryItem Item { get; private set; }

        public InventoryController CurrentController;

        private Canvas _canvas;

        private RectTransform _canvasRect;

        private Image _image;

        private Vector2 _offset;

        public InventoryDraggedItem(Canvas canvas, InventoryController originalController, Vector2Int originalPoint, IInventoryItem item, Vector2 offset)
        {
            OriginalController = originalController;
            CurrentController = OriginalController;
            OriginalPoint = originalPoint;
            Item = item;

            _canvas = canvas;
            _canvasRect = canvas.transform as RectTransform;

            _offset = offset;

            // Create an image representing the dragged item
            _image = new GameObject("DraggedItem").AddComponent<Image>();

            _image.raycastTarget = false;
            _image.transform.SetParent(_canvas.transform);
            _image.transform.SetAsLastSibling();
            _image.transform.localScale = Vector3.one;
            _image.sprite = item.Sprite;
            _image.SetNativeSize();
        }

        public Vector2 Position
        {
            set
            {
                var camera = _canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : _canvas.worldCamera;

                RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasRect, value + _offset, camera, out var newValue);

                _image.rectTransform.localPosition = newValue;

                if (CurrentController != null)
                {
                    Item.Position = CurrentController.ScreenToGrid(value + _offset + GetDraggedItemOffset(CurrentController.InventoryRenderer, Item));

                    var canAdd = CurrentController.Inventory.CanAddAt(Item, Item.Position) || CanSwap();

                    CurrentController.InventoryRenderer.SelectItem(Item, !canAdd, Color.white);
                }

                // Slowly animate the item towards the centre of the mouse pointer
                _offset = Vector2.Lerp(_offset, Vector2.zero, Time.deltaTime * 10f);
            }
        }

        public DropMode Drop(Vector2 position)
        {
            DropMode mode;

            if (CurrentController != null)
            {
                var grid = CurrentController.ScreenToGrid(position + _offset + GetDraggedItemOffset(CurrentController.InventoryRenderer, Item));

                if (CurrentController.Inventory.CanAddAt(Item, grid))
                {
                    CurrentController.Inventory.TryAddAt(Item, grid); // Place item in a new location

                    mode = DropMode.Added;
                }
                else if (CanSwap())
                {
                    var otherItem = CurrentController.Inventory.AllItems[0];

                    CurrentController.Inventory.TryRemove(otherItem);
                    OriginalController.Inventory.TryAdd(otherItem);
                    CurrentController.Inventory.TryAdd(Item);

                    mode = DropMode.Swapped;
                }
                else
                {
                    OriginalController.Inventory.TryAddAt(Item, OriginalPoint); // Return the item to its previous location

                    mode = DropMode.Returned;
                }

                CurrentController.InventoryRenderer.ClearSelection();
            }
            else
            {
                mode = DropMode.Dropped;

                if (!OriginalController.Inventory.TryForceDrop(Item)) // Drop the item on the ground
                {
                    OriginalController.Inventory.TryAddAt(Item, OriginalPoint);
                }
            }

            Object.Destroy(_image.gameObject);

            return mode;
        }

        private Vector2 GetDraggedItemOffset(InventoryRenderer renderer, IInventoryItem item)
        {
            var scale = new Vector2(Screen.width / _canvasRect.sizeDelta.x, Screen.height / _canvasRect.sizeDelta.y);

            var gx = -(item.Width * renderer.CellSize.x / 2f) + (renderer.CellSize.x / 2);
            var gy = -(item.Height * renderer.CellSize.y / 2f) + (renderer.CellSize.y / 2);

            return new Vector2(gx, gy) * scale;
        }

        private bool CanSwap()
        {
            if (!CurrentController.Inventory.CanSwap(Item))
            {
                return false;
            }

            var otherItem = CurrentController.Inventory.AllItems[0];

            return OriginalController.Inventory.CanAdd(otherItem) && CurrentController.Inventory.CanRemove(otherItem);
        }
    }
}


