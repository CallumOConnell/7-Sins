using Sins.Character;
using System.Collections;
using UnityEngine;

namespace Sins.Abilities
{
    public class StoneSkinAbility : MonoBehaviour
    {
        [SerializeField]
        private Ability _ability;

        [SerializeField]
        private int _damageToDecrease = 5;

        [SerializeField]
        private float _effectDuration = 5f;

        [SerializeField]
        private float _spawnDistance = 5f;

        [SerializeField]
        private GameObject _stoneWallPrefab;

        [SerializeField]
        private Transform _temporaryParent;

        private PlayerStats _playerStats;

        private void Awake()
        {
            _ability.OnAbilityUsed.AddListener(cooldown => Use());

            _ability.CanUse = true;

            _playerStats = GetComponent<PlayerStats>();
        }

        private IEnumerator ResetDuration(GameObject stoneWall)
        {
            yield return new WaitForSeconds(_effectDuration);

            _playerStats.Armour.RemoveModifier(_damageToDecrease);

            Destroy(stoneWall);
        }

        public void Use()
        {
            _playerStats.Armour.AddModifier(_damageToDecrease);

            var playerPos = transform.position;
            var playerDirection = transform.forward;

            var spawnPos = playerPos + playerDirection * _spawnDistance;

            var stoneWall = Instantiate(_stoneWallPrefab, spawnPos, Quaternion.LookRotation(playerDirection), _temporaryParent);

            stoneWall.transform.eulerAngles += new Vector3(0f, 90f, 0f);

            StartCoroutine(ResetDuration(stoneWall));
        }
    }
}