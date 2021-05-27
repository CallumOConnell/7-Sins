using Sins.Inventory;
using UnityEngine;

namespace Sins.UI
{
    public class MouseHover : MonoBehaviour
    {
        [SerializeField]
        private Texture2D _cursorEnemy;

        private void OnMouseEnter()
        {
            if (EquipmentManager.Instance.WeaponEquipped)
            {
                Cursor.SetCursor(_cursorEnemy, Vector2.zero, CursorMode.ForceSoftware);
            }
        }

        private void OnMouseExit() => Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

        private void OnDestroy() => Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}