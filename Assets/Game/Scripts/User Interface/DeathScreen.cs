using Sins.Character;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Sins.UI
{
    public class DeathScreen : MonoBehaviour
    {
        [SerializeField]
        private GameObject _deathScreen;

        [SerializeField]
        private CanvasGroup _hud;

        [SerializeField]
        private PlayerController _playerController;

        public void Show()
        {
            _playerController.MovementLocked = true;

            _hud.alpha = 0;
            _hud.interactable = false;
            _hud.blocksRaycasts = false;

            _deathScreen.SetActive(true);
        }

        public void Respawn()
        {
            SceneManager.LoadSceneAsync(3);
        }

        public void ReturnToMenu()
        {
            SceneManager.LoadSceneAsync(1);
        }
    }
}