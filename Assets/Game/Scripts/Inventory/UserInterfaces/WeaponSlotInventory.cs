using UnityEngine;

namespace Sins.Inventory
{
    public class WeaponSlotInventory : MonoBehaviour
    {
        [SerializeField]
        private InventoryRenderMode _renderMode = InventoryRenderMode.Single;

        [SerializeField]
        private int _maximumAlowedItemCount = 1;

        [SerializeField]
        private ItemType _allowedItem = ItemType.Weapon;

        [SerializeField]
        private int _width = 3;

        [SerializeField]
        private int _height = 3;

        [SerializeField]
        private EquipmentSlot _equipmentSlot;

        [SerializeField]
        private DropItem _dropItem;

        private void Start()
        {
            var provider = new InventoryProvider(_renderMode, _maximumAlowedItemCount, _allowedItem);

            // Create inventory
            var inventory = new InventoryManager(provider, _width, _height);

            // Render inventory
            GetComponent<InventoryRenderer>().SetInventory(inventory, provider.InventoryRenderMode);

            // Item has been dragged outside of inventory bounds so unequip the item and drop it on the ground
            inventory.OnItemDropped += (item) => _dropItem.Dropped(item as ItemDefinition);

            // Item has been added to an empty equipment slot so just equip the item
            // An item has been swapped with an existing item already in the equipment slot so unequip existing item and equip new item
            inventory.OnItemAdded += (item) => EquipmentManager.Instance.EquipWeapon(item as Weapon);

            // Item has been taken out of slot and put into an inventory slot so unequip the item
            inventory.OnItemRemoved += (item) => EquipmentManager.Instance.UnequipWeapon();
        }
    }
}