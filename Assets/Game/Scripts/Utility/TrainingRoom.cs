using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sins.Utils
{
    public class TrainingRoom : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> _enemies;

        [SerializeField]
        private GameObject _barrier;

        private void Start()
        {
            if (_enemies.Count > 0)
            {
                StartCoroutine(Monitor());
            }
        }

        private IEnumerator Monitor()
        {
            while (_enemies.Count != 0)
            {
                for (var i = 0; i < _enemies.Count; i++)
                {
                    var enemy = _enemies[i];

                    if (enemy == null)
                    {
                        _enemies.RemoveAt(i);
                    }
                }

                yield return new WaitForSeconds(3f);
            }

            Destroy(_barrier);
        }
    }
}