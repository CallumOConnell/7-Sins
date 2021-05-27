using UnityEngine;

namespace Sins.Utils
{
    public class RandomColourPicker : MonoBehaviour
    {
        private void Awake() => gameObject.GetComponent<SkinnedMeshRenderer>().material.color = Random.ColorHSV();
    }
}