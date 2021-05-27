using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Sins.UI
{
    [ExecuteInEditMode]
    public class Tooltip : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _headerText;

        [SerializeField]
        private TMP_Text _contentText;

        [SerializeField]
        private LayoutElement _layoutElement;

        [SerializeField]
        private int _characterWrapLimit;

        private RectTransform _rectTransform;

        private void Awake() => _rectTransform = GetComponent<RectTransform>();

        private void Update()
        {
            if (Application.isEditor) // Allows you to preview tooltip in editor
            {
                var headerLength = _headerText.text.Length;
                var contentLength = _contentText.text.Length;

                _layoutElement.enabled = headerLength > _characterWrapLimit || contentLength > _characterWrapLimit;
            }

            _rectTransform.pivot = CalculateRectPivot();

            transform.position = GetMousePosition();
        }

        private enum ScreenSide
        {
            TopLeftCorner,
            BottomLeftCorner,
            BottomRightCorner,
            TopRightCorner
        }

        private ScreenSide _screenSide;

        private Vector2 CalculateRectPivot()
        {
            var pivotX = GetMousePosition().x / Screen.width;
            var pivotY = GetMousePosition().y / Screen.height;
            var x = Mathf.Round(pivotX * 10f); // old 0 to 1.0 => 0 to 10
            var y = Mathf.Round(pivotY * 10f); // old 0 to 1.0 => 0 to 10

            if (x < 8 && y > 5)
            {
                _screenSide = ScreenSide.TopLeftCorner;
            }
            else if (x > 7 && y > 5)
            {
                _screenSide = ScreenSide.TopRightCorner;
            }
            else if (x > 7 && y < 7)
            {
                _screenSide = ScreenSide.BottomRightCorner;
            }
            else if (x < 9 && y < 7)
            {
                _screenSide = ScreenSide.BottomLeftCorner;
            }

            switch (_screenSide)
            {
                case ScreenSide.TopLeftCorner:
                    return new Vector2(-0.1f, 1.5f);
                case ScreenSide.BottomLeftCorner:
                    return new Vector2(-0.1f, -0.5f);
                case ScreenSide.BottomRightCorner:
                    return new Vector2(1.1f, -0.5f);
                case ScreenSide.TopRightCorner:
                    return new Vector2(1.1f, 1.5f);
                default:
                    break;
            }

            return Vector2.zero;
        }

        private Vector2 GetMousePosition() => Input.mousePosition;

        public void SetText(string content, string header = "")
        {
            if (string.IsNullOrEmpty(header))
            {
                _headerText.gameObject.SetActive(false);
            }
            else
            {
                _headerText.gameObject.SetActive(true);
                _headerText.text = header;
            }

            _contentText.text = content;

            var headerLength = _headerText.text.Length;
            var contentLength = _contentText.text.Length;

            _layoutElement.enabled = headerLength > _characterWrapLimit || contentLength > _characterWrapLimit;
        }
    }
}