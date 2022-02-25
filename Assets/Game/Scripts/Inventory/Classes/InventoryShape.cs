using System;
using UnityEngine;

namespace Sins.Inventory
{
    [Serializable]
    public class InventoryShape
    {
        [SerializeField]
        private int _width;

        [SerializeField]
        private int _height;

        [SerializeField]
        bool[] _shape;

        public int Width => _width;

        public int Height => _height;

        public InventoryShape(int width, int height)
        {
            _width = width;
            _height = height;
            _shape = new bool[_width * _height];
        }

        public InventoryShape(bool[, ] shape)
        {
            _width = shape.GetLength(0);
            _height = shape.GetLength(1);
            _shape = new bool[_width * _height];

            for (var x = 0; x < _width; x++)
            {
                for (var y = 0; y < _height; y++)
                {
                    _shape[GetIndex(x, y)] = shape[x, y];
                }
            }
        }

        public bool IsPartOfShape(Vector2Int localPoint)
        {
            if (localPoint.x < 0 || localPoint.x >= _width || localPoint.y < 0 || localPoint.y >= _height)
            {
                return false;
            }

            var index = GetIndex(localPoint.x, localPoint.y);

            return _shape[index];
        }

        private int GetIndex(int x, int y)
        {
            y = _height - 1 - y;

            return x + _width * y;
        }
    }
}