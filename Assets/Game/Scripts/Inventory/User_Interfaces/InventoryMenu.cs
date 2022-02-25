using UnityEngine;
using Sins.Character;

namespace Sins.Inventory
{
    public class InventoryMenu : MonoBehaviour
    {
        [SerializeField]
        private PlayerController _playerController;

        [SerializeField]
        private CanvasGroup _inventoryMenu;

        [SerializeField]
        private CanvasGroup _hud;

        public bool InventoryOpen { get; set; }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (InventoryOpen)
                {
                    CloseInventory();
                }
                else
                {
                    OpenInventory();
                }
            }
        }

        private void OpenInventory()
        {
            InventoryOpen = true;

            _hud.alpha = 0;
            _hud.interactable = false;
            _hud.blocksRaycasts = false;

            _inventoryMenu.alpha = 1;
            _inventoryMenu.interactable = true;
            _inventoryMenu.blocksRaycasts = true;

            _playerController.MovementLocked = true;    
        }

        private void CloseInventory()
        {
            InventoryOpen = false;

            _inventoryMenu.alpha = 0;
            _inventoryMenu.interactable = false;
            _inventoryMenu.blocksRaycasts = false;

            _hud.alpha = 1;
            _hud.interactable = true;
            _hud.blocksRaycasts = true;

            _playerController.MovementLocked = false;
        }
    }
}