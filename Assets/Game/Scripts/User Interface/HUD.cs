using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sins.Level;
using Sins.Character;

namespace Sins.UI
{
    public class HUD : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _levelText;

        [SerializeField]
        private Image _experienceBarImage;

        [SerializeField]
        private Slider _healthBar;

        [SerializeField]
        private PlayerStats _playerStats;

        private LevelSystemAnimated _levelSystemAnimated;

        private void Awake()
        {
            _playerStats.OnHealthValueChanged += SetHealthBarSize;
            _playerStats.OnMaxHealthValueChanged += SetHealthBarMaxSize;
        }

        public void SetLevelSystemAnimated(LevelSystemAnimated levelSystemAnimated)
        {
            // Set the LevelSystem object
            _levelSystemAnimated = levelSystemAnimated;

            // Update the starting values
            SetLevelNumber(_levelSystemAnimated.Level);
            SetExperienceBarSize(_levelSystemAnimated.GetExperienceNormalized());

            // Subscribe to the changed events
            _levelSystemAnimated.OnExperienceChanged += OnExperienceChanged;
            _levelSystemAnimated.OnLevelChanged += OnLevelChanged;
        }

        private void SetExperienceBarSize(float experienceNormalized) => _experienceBarImage.fillAmount = experienceNormalized;

        private void SetLevelNumber(int levelNumber) => _levelText.text = $"Level: {levelNumber}";

        // Level changed, update text
        private void OnLevelChanged(object sender, System.EventArgs e) => SetLevelNumber(_levelSystemAnimated.Level);

        // Experience changed, update bar size
        private void OnExperienceChanged(object sender, System.EventArgs e) => SetExperienceBarSize(_levelSystemAnimated.GetExperienceNormalized());

        public void SetHealthBarSize(int health) => _healthBar.value = health;

        public void SetHealthBarMaxSize(int maxHealth) => _healthBar.maxValue = maxHealth;
    }
}