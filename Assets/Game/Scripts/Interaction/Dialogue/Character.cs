using UnityEngine;

namespace Sins.Interaction
{
    [CreateAssetMenu(fileName = "New Character", menuName = "Character")]
    public class Character : ScriptableObject
    {
        public string Name;
        public Sprite Portrait;
    }
}