using System.Collections;
using UnityEngine;

namespace Sins.Utils
{
    public class TrainingRoom : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] _enemies;

        [SerializeField]
        private GameObject _barrier;

        private int _deadEnemies = 0;

        private void Start()
        {
            if (_enemies.Length > 0)
            {
                StartCoroutine(Monitor());
            }
        }

        private IEnumerator Monitor()
        {
            while (_deadEnemies != _enemies.Length)
            {
                foreach (var enemy in _enemies)
                {
                    if (enemy == null)
                    {
                        _deadEnemies++;
                    }
                }

                yield return new WaitForSeconds(3f);
            }

            Destroy(_barrier);
        }
    }
}