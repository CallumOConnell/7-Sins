using System.Collections.Generic;
using UnityEngine;

namespace Sins.Character
{
    [System.Serializable]
    public class Stat
    {
        [SerializeField]
        private int _baseValue;

        private List<int> _modifiers = new List<int>();

        public delegate void OnModifierValueChanged();

        public OnModifierValueChanged OnValueChanged;

        public int GetValue()
        {
            var finalValue = _baseValue;

            _modifiers.ForEach(x => finalValue += x);

            return finalValue;
        }

        public void AddModifier(int modifier)
        {
            if (modifier != 0)
            {
                _modifiers.Add(modifier);

                if (OnValueChanged != null)
                {
                    OnValueChanged.Invoke();
                }
            }
        }

        public void RemoveModifier(int modifier)
        {
            if (modifier != 0)
            {
                _modifiers.Remove(modifier);

                if (OnValueChanged != null)
                {
                    OnValueChanged.Invoke();
                }
            }
        }
    }
}