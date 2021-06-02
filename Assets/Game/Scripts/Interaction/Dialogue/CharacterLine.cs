using UnityEngine;
using TMPro;

namespace Sins.Interaction
{
    [System.Serializable]
    public struct CharacterLine
    {
        public Character Character;

        [TextArea(2, 5)]
        public string Text;
    }
}