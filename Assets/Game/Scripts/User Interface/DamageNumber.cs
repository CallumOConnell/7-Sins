using UnityEngine;

namespace Sins.UI
{
    public class DamageNumber : MonoBehaviour
    {
        private void Start() => Destroy(gameObject, 0.5f);

        private void LateUpdate() => transform.LookAt(transform.position + Camera.main.transform.forward);
    }
}