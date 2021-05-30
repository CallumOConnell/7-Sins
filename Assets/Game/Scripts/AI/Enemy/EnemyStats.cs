using Sins.AI;
using Sins.Interaction;
using Sins.Loot;
using UnityEngine;
using TMPro;
using Sins.Inventory;
using System;

namespace Sins.Character
{
    public class EnemyStats : CharacterStats
    {
        [SerializeField]
        private EnemyType _type;

        [SerializeField]
        private LootTable _lootTable;

        [SerializeField]
        private float _lootItemDropRadius = 5f;

        [SerializeField]
        private LayerMask _groundLayer;

        [SerializeField]
        private Transform _damagePopup;

        [SerializeField]
        private Transform _temporaryParent;

        public EnemyType Type => _type;

        public override void Die()
        {
            base.Die();

            Player.Instance.AddExperience(5);

            Destroy(gameObject);

            DropLootItem();
        }

        private void Start() => OnCharacterDamaged += DamageIndicator;

        private void DamageIndicator(int damageAmount)
        {
            var spawnPosition = transform.position;

            spawnPosition.y += 0.5f;

            var damagePoints = Instantiate(_damagePopup, spawnPosition, transform.rotation, _temporaryParent);

            damagePoints.GetComponent<TextMeshPro>().SetText(damageAmount.ToString());
        }

        private ItemDefinition CalculateItemStats(ItemDefinition item)
        {
            // Select item rarity
            var values = Enum.GetValues(typeof(ItemRarity));

            var rarity = UnityEngine.Random.Range(0, values.Length);

            item.Rarity = (ItemRarity)rarity;

            // Check if item is weapon or armour
            if (item is Weapon)
            {
                var weapon = item as Weapon;

                weapon.AttackDamageModifier = GenerateStat(item.Rarity);
                weapon.AttackSpeedModifier = GenerateStat(item.Rarity);
            }
            else
            {
                var armour = item as Armour;

                armour.MaxHealthModifier = GenerateStat(item.Rarity);
                armour.ArmourModifier = GenerateStat(item.Rarity);
                armour.MovementSpeedModifier = GenerateStat(item.Rarity);
            }

            return item;
        }

        private int GenerateStat(ItemRarity rarity)
        {
            switch (rarity)
            {
                case ItemRarity.Common:
                {
                    return UnityEngine.Random.Range(0, 100);
                }

                case ItemRarity.Uncommon:
                {
                    return UnityEngine.Random.Range(100, 200);
                }

                case ItemRarity.Rare:
                {
                    return UnityEngine.Random.Range(200, 300);
                }

                case ItemRarity.Epic:
                {
                    return UnityEngine.Random.Range(400, 500);
                }

                case ItemRarity.Legendary:
                {
                    return UnityEngine.Random.Range(500, 600);
                }

                default:
                {
                    return 0;
                }
            }
        }

        private void DropLootItem()
        {
            var randomItem = _lootTable.SelectLootItem();

            var item = CalculateItemStats(randomItem);

            // Get a random direction between 0 and 360 degrees
            var angle = UnityEngine.Random.Range(0, Mathf.PI * 2);

            var x = Mathf.Sin(angle) * _lootItemDropRadius;
            var y = GetGroundHeight(transform.position);
            var z = Mathf.Cos(angle) * _lootItemDropRadius;

            var itemPosition = new Vector3(x, y, z);

            itemPosition.x = transform.position.x - itemPosition.x;
            itemPosition.z = transform.position.z - itemPosition.z;

            var lootItem = Instantiate(randomItem.Prefab, itemPosition, Quaternion.identity);

            lootItem.GetComponent<PickupItem>().Item = item;
        }

        private float GetGroundHeight(Vector3 position)
        {
            position += new Vector3(0, 100, 0);

            if (Physics.Raycast(position, Vector3.down, out RaycastHit hit, 1000, _groundLayer))
            {
                return hit.point.y + 0.125f;
            }

            return Mathf.NegativeInfinity;
        }
    }
}