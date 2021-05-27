using UnityEngine;

namespace Sins.Character
{
    [RequireComponent(typeof(CharacterStats))]
    public class CharacterCombat : MonoBehaviour
    {
        public event System.Action OnAttack;

        public virtual void Attack(CharacterStats targetStats)
        {
            if (OnAttack != null)
            {
                OnAttack.Invoke();
            }
        }
    }
}