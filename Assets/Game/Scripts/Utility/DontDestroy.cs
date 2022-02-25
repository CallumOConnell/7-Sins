using UnityEngine;

namespace Sins.Utils
{
    public class DontDestroy : MonoBehaviour
    {
        private void Awake() => DontDestroyOnLoad(gameObject);
    }
}