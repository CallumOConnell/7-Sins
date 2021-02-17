using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sins.Utils;
using System;

namespace Sins.Inventory
{
    public class InventoryRenderer : MonoBehaviour
    {
        [SerializeField]
        private Vector2Int _cellSize;

        [SerializeField]
        private Sprite _cellSpriteEmpty;

        [SerializeField]
        private Sprite _cellSpriteSelected;

        [SerializeField]
        private Sprite _cellSpriteBlocked;

        internal IInventoryManager _inventory;

        private InventoryRenderMode _renderMode;

        private bool _listenersSet;

        private Pool<Image> _imagePool;

        private Image[] _grids;

        private Dictionary<IInventoryItem, Image> _items = new Dictionary<IInventoryItem, Image>();

        public RectTransform RectTransform { get; private set; }

        public Vector2 CellSize => _cellSize;

        private void Awake()
        {
            RectTransform = GetComponent<RectTransform>();

            // Create the image container
            RectTransform imageContainer = new GameObject("Image Pool").AddComponent<RectTransform>();

            imageContainer.transform.SetParent(transform);
            imageContainer.transform.localPosition = Vector3.zero;
            imageContainer.transform.localScale = Vector3.one;

            // Create pool of images
            _imagePool = new Pool<Image>(
                delegate
                {
                    Image image = new GameObject("Image").AddComponent<Image>();

                    image.transform.SetParent(imageContainer);
                    image.transform.localScale = Vector3.one;

                    return image;
                });
        }

        private void OnEnable()
        {
            if (_inventory != null && !_listenersSet)
            {
                if (_cellSpriteEmpty == null) throw new NullReferenceException("Sprite for empty cell is null.");
                if (_cellSpriteSelected == null) throw new NullReferenceException("Sprite for selected cells is null.");
                if (_cellSpriteBlocked == null) throw new NullReferenceException("Sprite for blocked cells is null.");

                _inventory.OnRebuilt += ReRenderAllItems;
                _inventory.OnItemAdded += HandleItemAdded;
                _inventory.OnItemRemoved += HandleItemRemoved;
                _inventory.OnItemDropped += HandleItemRemoved;
                _inventory.OnResized += HandleResized;

                _listenersSet = true;

                ReRenderGrid();
                ReRenderAllItems();
            }
        }

        private void OnDisable()
        {
            if (_inventory != null && _listenersSet)
            {
                _inventory.OnRebuilt -= ReRenderAllItems;
                _inventory.OnItemAdded -= HandleItemAdded;
                _inventory.OnItemRemoved -= HandleItemRemoved;
                _inventory.OnItemDropped -= HandleItemRemoved;
                _inventory.OnResized -= HandleResized;

                _listenersSet = false;
            }
        }

        private void ReRenderGrid()
        {
            if (_grids != null)
            {
                for (int i = 0; i < _grids.Length; i++)
                {
                    _grids[i].gameObject.SetActive(false);

                    RecycleImage(_grids[i]);

                    _grids[i].transform.SetSiblingIndex(i);
                }
            }

            _grids = null;

            Vector2 containerSize = new Vector2(_cellSize.x * _inventory.Width, _cellSize.y * _inventory.Height);

            Image grid;

            switch (_renderMode)
            {
                case InventoryRenderMode.Single:
                {
                    grid = CreateImage(_cellSpriteEmpty, true);

                    grid.rectTransform.SetAsFirstSibling();
                    grid.type = Image.Type.Sliced;
                    grid.rectTransform.localPosition = Vector3.zero;
                    grid.rectTransform.sizeDelta = containerSize;

                    _grids = new[] { grid };

                    break;
                }

                default:
                {
                    Vector3 topLeftCorner = new Vector3(-containerSize.x / 2, -containerSize.y / 2, 0);

                    Vector3 halfCellSize = new Vector3(_cellSize.x / 2, _cellSize.y / 2, 0);

                    _grids = new Image[_inventory.Width * _inventory.Height];

                    int counter = 0;

                    for (int y = 0; y < _inventory.Height; y++)
                    {
                        for (int x = 0; x < _inventory.Width; x++)
                        {
                            grid = CreateImage(_cellSpriteEmpty, true);

                            grid.gameObject.name = $"Grid {counter}";
                            grid.rectTransform.SetAsFirstSibling();
                            grid.type = Image.Type.Sliced;
                            grid.rectTransform.localPosition = topLeftCorner + new Vector3(_cellSize.x * (_inventory.Width - 1 - x), _cellSize.y * y, 0) + halfCellSize;
                            grid.rectTransform.sizeDelta = _cellSize;

                            _grids[counter] = grid;

                            counter++;
                        }
                    }

                    break;
                }
            }

            RectTransform.sizeDelta = containerSize;
        }

        private void ReRenderAllItems()
        {
            foreach (Image image in _items.Values)
            {
                image.gameObject.SetActive(false);

                RecycleImage(image);
            }

            _items.Clear();

            foreach (IInventoryItem item in _inventory.AllItems)
            {
                HandleItemAdded(item);
            }
        }

        private void HandleItemAdded(IInventoryItem item)
        {
            Image image = CreateImage(item.Sprite, false);

            if (_renderMode == InventoryRenderMode.Single)
            {
                image.rectTransform.localPosition = RectTransform.rect.center;
            }
            else
            {
                image.rectTransform.localPosition = GetItemOffset(item);
            }

            _items.Add(item, image);
        }

        private void HandleItemRemoved(IInventoryItem item)
        {
            if (_items.ContainsKey(item))
            {
                Image image = _items[item];

                image.gameObject.SetActive(false);

                RecycleImage(image);

                _items.Remove(item);
            }
        }

        private void HandleResized()
        {
            ReRenderGrid();
            ReRenderAllItems();
        }

        private Image CreateImage(Sprite sprite, bool raycastTarget)
        {
            Image image = _imagePool.Take();

            image.gameObject.SetActive(true);
            image.sprite = sprite;
            image.rectTransform.sizeDelta = new Vector2(image.sprite.rect.width, image.sprite.rect.height);
            image.transform.SetAsLastSibling();
            image.type = Image.Type.Simple;
            image.raycastTarget = raycastTarget;

            return image;
        }

        private void RecycleImage(Image image)
        {
            image.gameObject.name = "Image";
            image.gameObject.SetActive(false);

            _imagePool.Recycle(image);
        }

        internal Vector2 GetItemOffset(IInventoryItem item)
        {
            float x = (-(_inventory.Width * 0.5f) + item.Position.x + item.Width * 0.5f) * _cellSize.x;
            float y = (-(_inventory.Height * 0.5f) + item.Position.y + item.Height * 0.5f) * _cellSize.y;

            return new Vector2(x, y);
        }

        public void SetInventory(IInventoryManager inventoryManager, InventoryRenderMode renderMode)
        {
            OnDisable();

            _inventory = inventoryManager ?? throw new ArgumentNullException(nameof(inventoryManager));

            _renderMode = renderMode;

            OnEnable();
        }

        public void ClearSelection()
        {
            for (var i = 0; i < _grids.Length; i++)
            {
                _grids[i].sprite = _cellSpriteEmpty;
                _grids[i].color = Color.white;
            }
        }

        public void SelectItem(IInventoryItem item, bool blocked, Color color)
        {
            if (item == null) return;

            ClearSelection();

            switch (_renderMode)
            {
                case InventoryRenderMode.Single:
                {
                    _grids[0].sprite = blocked ? _cellSpriteBlocked : _cellSpriteSelected;
                    _grids[0].color = color;

                    break;
                }

                default:
                {
                    for (var x = 0; x < item.Width; x++)
                    {
                        for (var y = 0; y < item.Height; y++)
                        {
                            if (item.IsPartOfShape(new Vector2Int(x, y)))
                            {
                                var p = item.Position + new Vector2Int(x, y);

                                if (p.x >= 0 && p.x < _inventory.Width && p.y >= 0 && p.y < _inventory.Height)
                                {
                                    var index = p.y * _inventory.Width + (_inventory.Width - 1 - p.x);

                                    _grids[index].sprite = blocked ? _cellSpriteBlocked : _cellSpriteSelected;
                                    _grids[index].color = color;
                                }
                            }
                        }
                    }

                    break;
                }
            }
        }
    }
}