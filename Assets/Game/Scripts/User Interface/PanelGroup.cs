using UnityEngine;

namespace Sins.UI
{
    public class PanelGroup : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] _panels;

        [SerializeField]
        private TabGroup _tabGroup;

        private int _panelIndex;

        private void Awake() => ShowCurrentPanel();

        private void ShowCurrentPanel()
        {
            for (var i = 0; i < _panels.Length; i++)
            {
                if (i == _panelIndex)
                {
                    _panels[i].gameObject.SetActive(true);
                }
                else
                {
                    _panels[i].gameObject.SetActive(false);
                }
            }
        }

        public void SetPageIndex(int index)
        {
            _panelIndex = index;

            ShowCurrentPanel();
        }
    }
}