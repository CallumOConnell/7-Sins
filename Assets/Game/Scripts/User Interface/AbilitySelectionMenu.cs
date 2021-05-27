using Sins.Character;
using Sins.UI;
using UnityEngine;

namespace Sins.Abilities
{
    public class AbilitySelectionMenu : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup _abilitySelectionMenu;

        [SerializeField]
        private PlayerController _playerController;

        [SerializeField]
        private TabGroup _tabGroup;

        [SerializeField]
        private CanvasGroup _hud;

        private bool _isOpen = false;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.U))
            {
                if (_isOpen)
                {
                    Close();
                }
                else
                {
                    Open();
                }

                _isOpen = !_isOpen;
            }
        }

        private void Open()
        {
            _playerController.MovementLocked = true;

            _hud.alpha = 0;
            _hud.interactable = false;
            _hud.blocksRaycasts = false;

            _abilitySelectionMenu.alpha = 1;
            _abilitySelectionMenu.interactable = true;
            _abilitySelectionMenu.blocksRaycasts = true;

            _tabGroup.Initialise();
        }

        private void Close()
        {
            _playerController.MovementLocked = false;

            _abilitySelectionMenu.alpha = 0;
            _abilitySelectionMenu.interactable = false;
            _abilitySelectionMenu.blocksRaycasts = false;

            _hud.alpha = 1;
            _hud.interactable = true;
            _hud.blocksRaycasts = true;
        }
    }
}