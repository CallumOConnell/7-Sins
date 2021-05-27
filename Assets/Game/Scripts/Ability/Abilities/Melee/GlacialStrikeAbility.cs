﻿using Sins.AI;
using Sins.Character;
using UnityEngine;

namespace Sins.Abilities
{
    public class GlacialStrikeAbility : MonoBehaviour
    {
        [SerializeField]
        private Ability _ability;

        [SerializeField]
        private int _minimumDamage = 1;

        [SerializeField]
        private int _maximumDamage = 3;

        [SerializeField]
        private int _freezeChance = 25;

        [SerializeField]
        private float _radius = 10;

        [SerializeField]
        private Camera _camera;

        [SerializeField]
        private LayerMask _enemyMask;

        [SerializeField]
        private LayerMask _groundMask;

        private void Awake()
        {
            _ability.OnAbilityUsed.AddListener(cooldown => Use());

            _ability.CanUse = true;
        }

        public void Use()
        {
            Debug.Log("Glacial Strike Ability Activated");

            var ray = _camera.ScreenPointToRay(Input.mousePosition);

            var mousePosition = Vector3.zero;

            if (Physics.Raycast(ray, out var raycastHit, float.MaxValue, _groundMask))
            {
                mousePosition = raycastHit.point;
            }

            var colliders = Physics.OverlapSphere(mousePosition, _radius, _enemyMask);

            if (colliders.Length > 0)
            {
                foreach (var collider in colliders)
                {
                    if (collider.gameObject != null)
                    {
                        var damage = Random.Range(_minimumDamage, _maximumDamage);

                        var randomValue = Random.Range(0, 100);

                        if (randomValue <= _freezeChance)
                        {
                            // Freeze enemy effect here

                            collider.gameObject.GetComponent<EnemyController>().Stun();
                        }

                        collider.gameObject.GetComponent<EnemyStats>().Damage(damage);
                    }
                }
            }
        }
    }
}