using UnityEngine;
using UnityEngine.Events;

namespace Sins.Abilities
{
    [CreateAssetMenu(fileName = "Ability", menuName = "Ability Tree/Ability", order = 1)]
    public class Ability : ScriptableObject
    {
        public string Title; // Name of the ability

        [TextArea(10, 10)]
        public string Description; // A description of what the ability does

        public Sprite Icon; // Icon displayed in ability selection menu and ability hot bar

        public float CooldownTime; // The duration of the cooldown between uses

        public int LevelRequirement; // The level the player needs to be to unlock this ability

        public AbilityType Type; // The type of ability that falls in one of three categories

        public bool CanUse { get; set; } = true;

        public class FloatEvent : UnityEvent<float> { }

        public FloatEvent OnAbilityUsed = new FloatEvent();

        public void ActivateAbility()
        {
            Debug.Log($"Used {Title} Ability");

            OnAbilityUsed.Invoke(CooldownTime);

            AbilityCooldownManager.Instance.StartCooldown(this);
        }
    }
}