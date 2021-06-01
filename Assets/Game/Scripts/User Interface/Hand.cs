using UnityEngine;
using UnityEngine.UI;

namespace Sins.UI
{
    public class Hand : MonoBehaviour
    {
        public static Hand Instance { get; private set; }

        public IMoveable Moveable { get; set; }

        private Image _icon;

        public void TakeMoveable(IMoveable moveable)
        {
            Moveable = moveable;

            _icon.sprite = moveable.Icon;
            _icon.color = Color.white;
        }

        public IMoveable Put()
        {
            var temp = Moveable;

            Moveable = null;

            _icon.color = new Color(0, 0, 0, 0);

            return temp;
        }

        private void Update() => _icon.transform.position = Input.mousePosition;

        private void Awake()
        {
            Instance = this;

            _icon = GetComponent<Image>();
        }
    }
}