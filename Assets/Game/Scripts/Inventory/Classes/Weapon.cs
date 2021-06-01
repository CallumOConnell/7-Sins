using Sins.Abilities;
using UnityEngine;

namespace Sins.Inventory
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Inventory/Weapon")]
    public class Weapon : Equipment
    {
        public AbilityType AttackType;

        public MeshRenderer Mesh;

        public int AttackDamageModifier;
        public int AttackSpeedModifier;

        public Vector3 PositionOffset;
        public Vector3 RotationOffset;
        public Vector3 ScaleOffset;
    }
}