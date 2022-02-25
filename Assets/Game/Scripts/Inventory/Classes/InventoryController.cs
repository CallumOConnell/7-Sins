using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Sins.Inventory
{
    public class InventoryController : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerExitHandler, IPointerEnterHandler, IInventoryController
    {
        public Action<IInventoryItem> OnItemHovered { get; set; }
        public Action OnItemHoveredExit { get; set; }
        public Action<IInventoryItem> OnItemPickedUp { get; set; }
        public Action<IInventoryItem> OnItemAdded { get; set; }
        public Action<IInventoryItem> OnItemSwapped { get; set; }
        public Action<IInventoryItem> OnItemReturned { get; set; }
        public Action<IInventoryItem> OnItemDropped { get; set; }

        internal InventoryRenderer InventoryRenderer;
        internal InventoryManager Inventory => (InventoryManager)InventoryRenderer._inventory;

        private static InventoryDraggedItem _draggedItem;

        private Canvas _canvas;

        private PointerEventData _currentEventData;

        private IInventoryItem _itemToDrag;
        private IInventoryItem _lastHoveredItem;

        private void Awake()
        {
            InventoryRenderer = GetComponent<InventoryRenderer>();

            if (InventoryRenderer == null) throw new NullReferenceException("Could not find a inventory renderer");

            var canvases = GetComponentsInParent<Canvas>();

            if (canvases.Length == 0) throw new NullReferenceException("Could not find a canvas for inventory controller");

            _canvas = canvases[canvases.Length - 1];
        }

        private void Update()
        {
            if (_currentEventData == null) return;

            if (_draggedItem == null)
            {
                var grid = ScreenToGrid(_currentEventData.position);

                var item = Inventory.GetAtPoint(grid);

                if (item == _lastHoveredItem) return;

                OnItemHovered?.Invoke(item);

                _lastHoveredItem = item;
            }
            else
            {
                _draggedItem.Position = _currentEventData.position;
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            InventoryRenderer.ClearSelection();

            if (_itemToDrag == null || _draggedItem != null) return;

            var localPosition = ScreenToLocalPositionInRenderer(eventData.position);

            var itemOffset = InventoryRenderer.GetItemOffset(_itemToDrag);

            var offset = itemOffset - localPosition;

            _draggedItem = new InventoryDraggedItem(
                _canvas,
                this,
                _itemToDrag.Position,
                _itemToDrag,
                offset
            );

            Inventory.TryRemove(_itemToDrag);

            OnItemPickedUp?.Invoke(_itemToDrag);
        }

        public void OnDrag(PointerEventData eventData) => _currentEventData = eventData;

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_draggedItem == null) return;

            var mode = _draggedItem.Drop(eventData.position);

            switch (mode)
            {
                case InventoryDraggedItem.DropMode.Added:
                {
                    OnItemAdded?.Invoke(_itemToDrag);

                    break;
                }
                case InventoryDraggedItem.DropMode.Swapped:
                {
                    OnItemSwapped?.Invoke(_itemToDrag);

                    break;
                }
                case InventoryDraggedItem.DropMode.Returned:
                {
                    OnItemReturned?.Invoke(_itemToDrag);

                    break;
                }
                case InventoryDraggedItem.DropMode.Dropped:
                {
                    OnItemDropped?.Invoke(_itemToDrag);

                    ClearHoveredItem();

                    break;
                }
            }

            _draggedItem = null;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_draggedItem != null) return;

            var grid = ScreenToGrid(eventData.position);

            _itemToDrag = Inventory.GetAtPoint(grid);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_draggedItem != null)
            {
                _draggedItem.CurrentController = this;
            }

            _currentEventData = eventData;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_draggedItem != null)
            {
                _draggedItem.CurrentController = null;

                InventoryRenderer.ClearSelection();
            }
            else
            {
                ClearHoveredItem();
            }

            _currentEventData = null;
        }

        private void ClearHoveredItem()
        {
            if (_lastHoveredItem != null)
            {
                OnItemHovered?.Invoke(null);
            }

            _lastHoveredItem = null;
        }

        private Vector2 ScreenToLocalPositionInRenderer(Vector2 screenPosition)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                InventoryRenderer.RectTransform,
                screenPosition,
                _canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : _canvas.worldCamera,
                out var localPosition
            );

            return localPosition;
        }

        // Get a point on the grid from a given screen point
        internal Vector2Int ScreenToGrid(Vector2 screenPoint)
        {
            var position = ScreenToLocalPositionInRenderer(screenPoint);
            var sizeDelta = InventoryRenderer.RectTransform.sizeDelta;

            position.x += sizeDelta.x / 2;
            position.y += sizeDelta.y / 2;

            return new Vector2Int(Mathf.FloorToInt(position.x / InventoryRenderer.CellSize.x), Mathf.FloorToInt(position.y / InventoryRenderer.CellSize.y));
        }
    }
}