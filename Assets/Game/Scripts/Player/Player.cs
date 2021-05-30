using UnityEngine;
using Sins.Inventory;
using Sins.Level;
using Sins.UI;

namespace Sins.Character
{
    public class Player : MonoBehaviour
    {
        [SerializeField]
        private PlayerInventory _playerInventory;

        [SerializeField]
        private HUD _hud;

        public PlayerInventory Inventory => _playerInventory;
        public int Level => _levelSystem.Level;
        public int Experience => _levelSystem.Experience;

        public static Player Instance { get; private set; }

        private LevelSystem _levelSystem;

        public void AddExperience(int amount) => _levelSystem.AddExperience(amount);

        private void Awake()
        {
            Instance = this;

            _levelSystem = new LevelSystem();

            var levelSystemAnimated = new LevelSystemAnimated(_levelSystem);

            _levelSystem.AddExperience(100);

            _hud.SetLevelSystemAnimated(levelSystemAnimated);
        }
    }
}