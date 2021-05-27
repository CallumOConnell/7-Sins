using UnityEngine;
using UnityEngine.AI;

namespace Sins.Character
{
    public class CharacterStats : MonoBehaviour
    {
        public int Health { get; private set; }

        public Stat MaxHealth;
        public Stat Armour;
        public Stat MovementSpeed;
        public Stat AttackDamage;
        public Stat AttackSpeed;

        public delegate void OnHealthChanged(int health);
        public delegate void OnMaxHealthChanged(int maxHealth);
        public delegate void OnDamaged(int damage);

        public OnHealthChanged OnHealthValueChanged;
        public OnMaxHealthChanged OnMaxHealthValueChanged;
        public OnDamaged OnCharacterDamaged;

        private NavMeshAgent _agent;

        private void Awake()
        {
            Health = MaxHealth.GetValue();

            _agent = GetComponent<NavMeshAgent>();

            MaxHealth.OnValueChanged += HealthChangedCallback;
            MovementSpeed.OnValueChanged += MovementSpeedChangedCallBack;
        }

        private void HealthChangedCallback()
        {
            if (OnMaxHealthValueChanged != null)
            {
                OnMaxHealthValueChanged.Invoke(MaxHealth.GetValue());
            }
        }

        private void MovementSpeedChangedCallBack() => _agent.speed = MovementSpeed.GetValue();

        public virtual void Die() { }

        public void Damage(int amount)
        {
            if (amount > 0)
            {
                amount -= Armour.GetValue();
                amount = Mathf.Clamp(amount, 0, int.MaxValue);

                Debug.Log($"{name} took {amount} damage");

                Health -= amount;

                if (OnHealthValueChanged != null)
                {
                    OnHealthValueChanged.Invoke(Health);
                }

                if (OnCharacterDamaged != null)
                {
                    OnCharacterDamaged.Invoke(amount);
                }

                if (Health <= 0)
                {
                    Die();
                }
            }
        }

        public void Heal(int amount)
        {
            if (amount > 0)
            {
                Health += amount;

                if (Health > MaxHealth.GetValue())
                {
                    Health = MaxHealth.GetValue();
                }

                if (OnHealthValueChanged != null)
                {
                    OnHealthValueChanged.Invoke(Health);
                }
            }
        }
    }
}