using UnityEngine;

namespace Sins.Inventory
{
    [CreateAssetMenu(fileName = "Item", menuName = "Inventory/Item", order = 1)]
    public class ItemDefinition : ScriptableObject, IInventoryItem
    {
        [SerializeField]
        private Sprite _sprite;

        [SerializeField]
        private InventoryShape _shape;

        [SerializeField]
        private ItemType _type = ItemType.Utility;

        [SerializeField]
        private bool _canDrop = true;

        [HideInInspector]
        private Vector2Int _position = Vector2Int.zero;

        public string Name => name;

        public ItemType Type => _type;

        public Sprite Sprite => _sprite;

        public int Width => _shape.Width;

        public int Height => _shape.Height;

        public Vector2Int Position { get => _position; set => _position = value; }

        public bool CanDrop => _canDrop;

        public bool IsPartOfShape(Vector2Int localPosition) => _shape.IsPartOfShape(localPosition);

        public IInventoryItem CreateInstance()
        {
            var clone = Instantiate(this);

            clone.name = clone.name.Substring(0, clone.name.Length - 7); // Remove (clone) from name

            return clone;
        }
    }
}