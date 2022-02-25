using UnityEngine;
using UnityEngine.SceneManagement;

namespace Sins.Utils
{
    public class SceneSwitch : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            SceneManager.LoadSceneAsync(3, LoadSceneMode.Single);
        }
    }
}