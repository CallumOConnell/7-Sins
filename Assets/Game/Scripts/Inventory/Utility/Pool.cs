using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Endure.Inventory
{
    public sealed class Pool<T> where T : class
    {
        private List<T> _inactive = new List<T>();
        private List<T> _active = new List<T>();

        private Func<T> _creator;

        private bool _allowTakingWhenEmpty;

        public int Count => _inactive.Count;

        public bool IsEmpty => Count == 0;

        public bool CanTake => _allowTakingWhenEmpty || !IsEmpty;

        public Pool(Func<T> creator, int initialCount = 0, bool allowTakingWhenEmpty = true)
        {
            _creator = creator;
            _inactive.Capacity = initialCount;
            _allowTakingWhenEmpty = allowTakingWhenEmpty;

            while (_inactive.Count < initialCount)
            {
                _inactive.Add(_creator());
            }
        }

        public T Take()
        {
            if (IsEmpty)
            {
                if (_allowTakingWhenEmpty)
                {
                    T obj = _creator();

                    _inactive.Add(obj);

                    return TakeInternal();
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return TakeInternal();
            }
        }

        public void Recycle(T item)
        {
            if (!_active.Contains(item))
            {
                Debug.LogError("An item was recycled even though it was not part of the pool.");
            }

            _inactive.Add(item);
            _active.Remove(item);
        }

        public List<T> GetInactive() => _inactive.ToList();

        public List<T> GetActive() => _active.ToList();

        private T TakeInternal()
        {
            T obj = _inactive[_inactive.Count - 1];

            _inactive.RemoveAt(_inactive.Count - 1);

            _active.Add(obj);

            return obj;
        }
    }
}