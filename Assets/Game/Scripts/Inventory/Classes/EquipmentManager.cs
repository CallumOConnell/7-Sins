using UnityEngine;

namespace Sins.Inventory
{
    public class EquipmentManager : MonoBehaviour
    {
        public static EquipmentManager Instance;

        public delegate void OnEquipmentChanged(Equipment newItem, Equipment oldItem);

        public OnEquipmentChanged OnEquipmentChangedCallBack;

        public bool WeaponEquipped { get; private set; }

        [SerializeField]
        private SkinnedMeshRenderer _targetMesh;

        [SerializeField]
        private Transform _targetHand;

        private Armour[] _currentArmour;

        private Weapon _currentWeapon;

        private SkinnedMeshRenderer[] _currentArmourMeshes;

        private MeshRenderer _currentWeaponMesh;

        private void Awake() => Instance = this;

        private void Start()
        {
            var numerOfSlots = System.Enum.GetNames(typeof(EquipmentSlot)).Length;

            _currentArmour = new Armour[numerOfSlots];
            _currentArmourMeshes = new SkinnedMeshRenderer[numerOfSlots];
        }

        private void SetArmourBlendShapes(Armour item, int weight)
        {
            foreach (var blendShape in item.CoveredMeshRegions)
            {
                _targetMesh.SetBlendShapeWeight((int)blendShape, weight);
            }
        }

        public void EquipWeapon(Weapon newItem)
        {
            // Unequip the item that is already in the equipment slot and return that item
            var oldItem = UnequipWeapon();

            // An item has been equipped so we invoke the callback
            if (OnEquipmentChangedCallBack != null)
            {
                OnEquipmentChangedCallBack.Invoke(newItem, oldItem);
            }

            // Insert the item into the slot
            _currentWeapon = newItem;

            // Create the weapon mesh and parent it to the hand
            var weaponMesh = Instantiate(newItem.Mesh, _targetHand);

            _currentWeaponMesh = weaponMesh;

            // Get the weapons transform
            var weaponTransform = weaponMesh.GetComponent<Transform>();

            // Reset the weapons transform
            weaponTransform.localPosition = Vector3.zero;
            weaponTransform.localEulerAngles = Vector3.zero;
            weaponTransform.localScale = Vector3.zero;

            // Set the weapons offset position
            weaponTransform.localPosition = newItem.PositionOffset;
            weaponTransform.localEulerAngles = newItem.RotationOffset;
            weaponTransform.localScale = newItem.ScaleOffset;

            WeaponEquipped = true;
        }

        public Weapon UnequipWeapon()
        {
            if (_currentWeapon != null)
            {
                if (_currentWeaponMesh != null)
                {
                    Destroy(_currentWeaponMesh.gameObject);
                }

                var oldItem = _currentWeapon;

                _currentWeapon = null;

                if (OnEquipmentChangedCallBack != null)
                {
                    OnEquipmentChangedCallBack.Invoke(null, oldItem);
                }

                WeaponEquipped = false;

                return oldItem;
            }

            return null;
        }

        public void EquipArmour(Armour newItem)
        {
            // Find out what slot the item fits in
            var slotIndex = (int)newItem.EquipSlot;

            // Unequip the item that is already in the equipment slot and return that item
            var oldItem = UnequipArmour(slotIndex);

            // An item has been equipped so we invoke the callback
            if (OnEquipmentChangedCallBack != null)
            {
                OnEquipmentChangedCallBack.Invoke(newItem, oldItem);
            }

            // Set the blendshapes for the player body based on the equipment equipped
            SetArmourBlendShapes(newItem, 100);

            // Insert the item into the slot
            _currentArmour[slotIndex] = newItem;

            // Create the armour mesh
            var newMesh = Instantiate(newItem.Mesh);

            // Set the armour's mesh parent as the players transform
            newMesh.transform.parent = _targetMesh.transform;

            // Set the bones of the armour mesh to those of the players
            newMesh.bones = _targetMesh.bones;
            newMesh.rootBone = _targetMesh.rootBone;

            _currentArmourMeshes[slotIndex] = newMesh;
        }

        public Armour UnequipArmour(int slotIndex)
        {
            if (_currentArmour[slotIndex] != null)
            {
                if (_currentArmourMeshes[slotIndex] != null)
                {
                    Destroy(_currentArmourMeshes[slotIndex].gameObject);
                }

                var oldItem = _currentArmour[slotIndex];

                SetArmourBlendShapes(oldItem, 0);

                _currentArmour[slotIndex] = null;

                if (OnEquipmentChangedCallBack != null)
                {
                    OnEquipmentChangedCallBack.Invoke(null, oldItem);
                }

                return oldItem;
            }

            return null;
        }
    }
}