using Sins.Inventory;
using Sins.UI;
using UnityEngine;

namespace Sins.Character
{
    public class PlayerStats : CharacterStats
    {
        [SerializeField]
        private DeathScreen _deathScreen;

        private void Start() => EquipmentManager.Instance.OnEquipmentChangedCallBack += OnEquipmentChanged;

        private void OnEquipmentChanged(Equipment newItem, Equipment oldItem)
        {
            if (newItem != null)
            {
                if (newItem is Weapon)
                {
                    var weapon = newItem as Weapon;

                    AttackDamage.AddModifier(weapon.AttackDamageModifier);
                    AttackSpeed.AddModifier(weapon.AttackSpeedModifier);
                }
                else
                {
                    var armour = newItem as Armour;

                    MaxHealth.AddModifier(armour.MaxHealthModifier);
                    Armour.AddModifier(armour.ArmourModifier);
                }
            }

            if (oldItem != null)
            {
                if (oldItem is Weapon)
                {
                    var weapon = oldItem as Weapon;

                    AttackDamage.RemoveModifier(weapon.AttackDamageModifier);
                    AttackSpeed.RemoveModifier(weapon.AttackSpeedModifier);
                }
                else
                {
                    var armour = oldItem as Armour;

                    MaxHealth.RemoveModifier(armour.MaxHealthModifier);
                    Armour.RemoveModifier(armour.ArmourModifier);
                }
            }
        }

        public override void Die()
        {
            base.Die();

            _deathScreen.Show();
        }
    }
}