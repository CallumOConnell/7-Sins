using UnityEngine;
using UnityEngine.SceneManagement;

namespace Sins.UI
{
    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject _homePanel;

        [SerializeField]
        private GameObject _optionsPanel;

        public void PlayGame() => SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);

        public void OpenOptionsMenu()
        {
            _homePanel.SetActive(false);
            _optionsPanel.SetActive(true);
        }

        public void QuitGame() => Application.Quit();
    }
}