using UnityEngine;

namespace Sins.Player
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField]
        private Transform _target;

        [SerializeField]
        private Vector3 _offset;

        [SerializeField]
        private float _currentZoom = 10f;

        [SerializeField]
        private float _pitch = 2f;

        private void LateUpdate()
        {
            transform.position = _target.position - _offset * _currentZoom;

            transform.LookAt(_target.position + Vector3.up * _pitch);
        }
    }
}