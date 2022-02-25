using System;

namespace Sins.Level
{
    public class LevelSystem
    {
        public event EventHandler OnExperienceChanged;
        public event EventHandler OnLevelChanged;

        public int Level { get; private set; }
        public int Experience { get; private set; }

        private static readonly int[] _experiencePerLevel = new[] { 100, 200, 300, 400, 500, 600, 700, 800, 900, 1000 };

        public LevelSystem()
        {
            Level = 0;
            Experience = 0;
        }

        public void AddExperience(int amount)
        {
            if (!IsMaxLevel(Level))
            {
                Experience += amount;

                // Player has enough experience to level up
                while (!IsMaxLevel(Level) && Experience >= GetExperienceToNextLevel(Level))
                {
                    Experience -= GetExperienceToNextLevel(Level);

                    Level++;

                    OnLevelChanged?.Invoke(this, EventArgs.Empty);
                }

                OnExperienceChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public float GetExperienceNormalized() => IsMaxLevel(Level) ? 1f : (float)Experience / GetExperienceToNextLevel(Level);

        public int GetExperienceToNextLevel(int level) => level < _experiencePerLevel.Length ? _experiencePerLevel[level] : -1;

        public bool IsMaxLevel(int level) => level == _experiencePerLevel.Length - 1;
    }
}