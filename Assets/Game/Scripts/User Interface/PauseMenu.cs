using Sins.Character;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Sins.UI
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField]
        private GameObject _pauseMenu;

        [SerializeField]
        private CanvasGroup _optionsMenu;

        [SerializeField]
        private CanvasGroup _hud;

        [SerializeField]
        private PlayerController _playerController;

        public bool GamePaused { get; private set; }

        public void PauseGame()
        {
            Time.timeScale = 0f;

            _playerController.MovementLocked = true;

            if (_hud != null)
            {
                _hud.alpha = 0;
                _hud.interactable = false;
                _hud.blocksRaycasts = false;
            }

            _pauseMenu.SetActive(true);

            GamePaused = true;
        }

        public void ResumeGame()
        {
            Time.timeScale = 1f;

            _playerController.MovementLocked = false;

            _pauseMenu.SetActive(false);

            if (_hud != null)
            {
                _hud.alpha = 1;
                _hud.interactable = true;
                _hud.blocksRaycasts = true;
            }

            GamePaused = false;
        }

        public void OpenOptionsMenu()
        {
            _pauseMenu.SetActive(false);

            _optionsMenu.alpha = 1f;
            _optionsMenu.interactable = true;
            _optionsMenu.blocksRaycasts = true;
        }

        public void ExitToTitleScreen() => SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);

        public void ExitToDesktop() => Application.Quit();

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (GamePaused)
                {
                    ResumeGame();
                }
                else
                {
                    PauseGame();
                }
            }
        }
    }
}