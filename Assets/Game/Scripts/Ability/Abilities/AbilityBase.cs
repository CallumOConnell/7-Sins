using UnityEngine;

namespace Sins.Abilities
{
    public class AbilityBase : MonoBehaviour
    {
        [SerializeField]
        private Ability _ability;

        private void Awake() => _ability.OnAbilityUsed.AddListener(cooldown => Use());

        public virtual void Use() {}
    }
}