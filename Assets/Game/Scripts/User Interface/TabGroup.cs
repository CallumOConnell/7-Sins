using System.Collections.Generic;
using UnityEngine;

namespace Sins.UI
{
    public class TabGroup : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> _tabPages = new List<GameObject>();

        [SerializeField]
        private Color _tabIdleColour;

        [SerializeField]
        private Color _tabTextIdleColour;

        [SerializeField]
        private Color _tabHoverColour;

        [SerializeField]
        private Color _tabTextHoverColour;

        [SerializeField]
        private Color _tabActiveColour;

        [SerializeField]
        private Color _tabTextActiveColour;

        private List<TabButton> _tabButtons = new List<TabButton>();

        private TabButton _selectedTab; 

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                PreviousTab();
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                NextTab();
            }
        }

        // Select the first tab as active tab when ui opens
        public void Initialise()
        {
            foreach (var button in _tabButtons)
            {
                if (button.transform.GetSiblingIndex() == 0)
                {
                    OnTabSelected(button);
                }
            }
        }

        public void Subcribe(TabButton button)
        {
            _tabButtons.Add(button);

            _tabButtons.Sort((x, y) => x.transform.GetSiblingIndex().CompareTo(y.transform.GetSiblingIndex()));
        }

        public void OnTabEnter(TabButton button)
        {
            ResetTabs();

            if (_selectedTab == null || button != _selectedTab)
            {
                button.Background.color = _tabHoverColour;
                button.Text.color = _tabTextHoverColour;
            }
        }

        public void OnTabExit(TabButton button) => ResetTabs();

        public void OnTabSelected(TabButton button)
        {
            if (_selectedTab != null)
            {
                _selectedTab.Deselect();
            }

            _selectedTab = button;

            _selectedTab.Select();

            ResetTabs();

            button.Background.color = _tabActiveColour;
            button.Text.color = _tabTextActiveColour;

            var index = button.transform.GetSiblingIndex();

            for (var i = 0; i < _tabPages.Count; i++)
            {
                if (i == index)
                {
                    _tabPages[i].SetActive(true);
                }
                else
                {
                    _tabPages[i].SetActive(false);
                }
            }
        }

        public void ResetTabs()
        {
            foreach (var button in _tabButtons)
            {
                if (_selectedTab != null && button == _selectedTab) continue;

                button.Background.color = _tabIdleColour;
                button.Text.color = _tabTextIdleColour;
            }
        }

        public void NextTab()
        {
            if (_selectedTab != null)
            {
                var currentIndex = _selectedTab.transform.GetSiblingIndex();

                var nextIndex = currentIndex < _tabButtons.Count - 1 ? currentIndex + 1 : _tabButtons.Count - 1;

                var tabButton = _tabButtons[nextIndex];

                if (tabButton != null)
                {
                    OnTabSelected(_tabButtons[nextIndex]);
                }
            }
        }

        public void PreviousTab()
        {
            if (_selectedTab != null)
            {
                var currentIndex = _selectedTab.transform.GetSiblingIndex();

                var previousIndex = currentIndex > 0 ? currentIndex - 1 : 0;

                var tabButton = _tabButtons[previousIndex];

                if (tabButton != null)
                {
                    OnTabSelected(_tabButtons[previousIndex]);
                }
            }
        }
    }
}