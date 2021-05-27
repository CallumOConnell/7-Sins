using Sins.Character;
using Sins.Inventory;
using UnityEngine;

namespace Sins.Interaction
{
    [RequireComponent(typeof(CharacterStats))]
    public class Enemy : Interactable
    {
        private Player _player;

        private EnemyStats _enemyStats;

        private void Start()
        {
            _player = Player.Instance;

            _enemyStats = GetComponent<EnemyStats>();
        }

        public override void Interact()
        {
            base.Interact();

            var playerCombat = _player.GetComponent<CharacterCombat>();

            if (playerCombat != null)
            {
                if (EquipmentManager.Instance.WeaponEquipped)
                {
                    playerCombat.Attack(_enemyStats);
                }
            }
        }
    }
}