using Sins.Inventory;
using UnityEngine;

namespace Sins.UI
{
    public class ItemTooltip : MonoBehaviour
    {
        private InventoryMenu _inventoryInput;

        private static LTDescr _delay;

        private void Start()
        {
            _inventoryInput = GetComponent<InventoryMenu>();

            var allControllers = FindObjectsOfType<InventoryController>();

            foreach (var controller in allControllers)
            {
                controller.OnItemHovered += HandleItemHover;
            }
        }

        private void HandleItemHover(IInventoryItem hoveredItem)
        {
            if (_inventoryInput.InventoryOpen)
            {
                var item = hoveredItem as ItemDefinition;      

                if (item != null)
                {
                    var itemDescription = GetItemDescription(item);

                    if (!string.IsNullOrEmpty(itemDescription))
                    {
                        _delay = LeanTween.delayedCall(0.5f, () =>
                        {
                            TooltipSystem.Show(itemDescription, item.Name);
                        });
                    }
                }
                else
                {
                    if (_delay != null)
                    {
                        LeanTween.cancel(_delay.uniqueId);
                    }

                    TooltipSystem.Hide();
                }
            }
        }

        private string GetItemDescription(ItemDefinition item)
        {
            var formattedDescription = $"{item.Description}\nItem Stats:\n";

            if (item.Type == ItemType.Weapon)
            {
                var weapon = item as Weapon;

                if (weapon.AttackDamageModifier > 0)
                {
                    var attackDamage = $"Attack Damage: {weapon.AttackDamageModifier}\n";

                    formattedDescription += attackDamage;
                }

                if (weapon.AttackSpeedModifier > 0)
                {
                    var attackSpeed = $"Attack Speed: {weapon.AttackSpeedModifier}\n";

                    formattedDescription += attackSpeed;
                }
            }
            else
            {
                var armour = item as Armour;

                if (armour.ArmourModifier > 0)
                {
                    var armourValue = $"Armour: {armour.ArmourModifier}\n";

                    formattedDescription += armourValue;
                }

                if (armour.MaxHealthModifier > 0)
                {
                    var maxHealth = $"Max Health: {armour.MaxHealthModifier}\n";

                    formattedDescription += maxHealth;
                }

                if (armour.MovementSpeedModifier > 0)
                {
                    var movementSpeed = $"Movement Speed: {armour.MovementSpeedModifier}\n";
                }
            }

            return formattedDescription;
        }
    }
}