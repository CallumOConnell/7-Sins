using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sins.Utils
{

    public class EnemyCheck : MonoBehaviour
    {
            [SerializeField]
            private List<GameObject> _enemies;

            [SerializeField]
            private GameObject MainUI;

            [SerializeField]
            private GameObject EndScreen;

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

                    yield return new WaitForSeconds(1f);
                }

                MainUI.SetActive(false);
                EndScreen.SetActive(true);

            }
    }

}