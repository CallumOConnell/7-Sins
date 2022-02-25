using Sins.Utils;
using System;
using UnityEngine;

namespace Sins.Level
{
    public class LevelSystemAnimated
    {
        private LevelSystem _levelSystem;

        private bool _isAnimating;

        private float _updateTimer;
        private float _updateTimerMax;

        public event EventHandler OnExperienceChanged;
        public event EventHandler OnLevelChanged;

        public int Level { get; private set; }
        public int Experience { get; private set; }

        public LevelSystemAnimated(LevelSystem levelSystem)
        {
            SetLevelSystem(levelSystem);

            _updateTimerMax = .010f;

            FunctionUpdater.Create(() => Update());
        }

        public void SetLevelSystem(LevelSystem levelSystem)
        {
            _levelSystem = levelSystem;

            Experience = levelSystem.Experience;

            levelSystem.OnExperienceChanged += LevelSystemAnimated_OnExperienceChanged;
            levelSystem.OnLevelChanged += LevelSystemAnimated_OnLevelChanged;
        }

        private void LevelSystemAnimated_OnLevelChanged(object sender, EventArgs e) => _isAnimating = true;

        private void LevelSystemAnimated_OnExperienceChanged(object sender, EventArgs e) => _isAnimating = true;

        private void Update()
        {
            if (_isAnimating)
            {
                _updateTimer += Time.deltaTime;

                while (_updateTimer > _updateTimerMax)
                {
                    _updateTimer -= _updateTimerMax;

                    UpdateAddExperience();
                }
            }
        }

        private void UpdateAddExperience()
        {
            if (Level < _levelSystem.Level)
            {
                AddExperience();
            }
            else
            {
                if (Experience < _levelSystem.Experience)
                {
                    AddExperience();
                }
                else
                {
                    _isAnimating = false;
                }
            }
        }

        private void AddExperience()
        {
            Experience++;

            if (Experience >= _levelSystem.GetExperienceToNextLevel(Level))
            {
                Level++;
                Experience = 0;

                OnLevelChanged?.Invoke(this, EventArgs.Empty);
            }

            OnExperienceChanged?.Invoke(this, EventArgs.Empty);
        }

        public float GetExperienceNormalized() => _levelSystem.IsMaxLevel(Level) ? 1f : (float)Experience / _levelSystem.GetExperienceToNextLevel(Level);
    }
}