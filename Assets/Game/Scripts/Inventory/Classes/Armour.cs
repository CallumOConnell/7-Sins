using UnityEngine;

namespace Sins.Inventory
{
    [CreateAssetMenu(fileName = "Armour", menuName = "Inventory/Armour")]
    public class Armour : Equipment
    {
        public SkinnedMeshRenderer Mesh;

        public EquipmentMeshRegion[] CoveredMeshRegions;

        public int MaxHealthModifier;
        public int ArmourModifier;
    }
}