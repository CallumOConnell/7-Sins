using UnityEngine;

namespace Sins.Interaction
{
    [CreateAssetMenu(fileName = "New Conversation", menuName = "Conversation")]
    public class Conversation : ScriptableObject
    {
        public Character SpeakerLeft;
        public Character SpeakerRight;
        public CharacterLine[] Lines;
    }
}