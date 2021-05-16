using UnityEngine;
using UnityEngine.SceneManagement;

namespace Sins.UI
{
    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject _homePanel;

        [SerializeField]
        private CanvasGroup _optionsPanel;

        [SerializeField]
        private TabGroup _tabGroup;

        public void PlayGame() => SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);

        public void OpenOptionsMenu()
        {
            _homePanel.SetActive(false);
            _optionsPanel.alpha = 1f;
            _optionsPanel.interactable = true;
            _optionsPanel.blocksRaycasts = true;
            _tabGroup.Initialise();
        }

        public void QuitGame() => Application.Quit();
    }
}