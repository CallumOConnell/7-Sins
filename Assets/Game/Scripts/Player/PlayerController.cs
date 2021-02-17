using UnityEngine;
using UnityEngine.AI;

namespace Sins.Player
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private GameObject _player;

        [SerializeField]
        private Camera _camera;

        [SerializeField]
        private LayerMask _groundLayer;

        private NavMeshAgent _agent;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var ray = _camera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit, 100, _groundLayer))
                {
                    _agent.SetDestination(hit.point);
                }
            }
        }
    }
}