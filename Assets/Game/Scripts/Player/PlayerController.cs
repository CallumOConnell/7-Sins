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

        private Transform _target;

        private NavMeshAgent _agent;

        private Interactable _focus;

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
                    Interactable interactable = hit.collider.GetComponent<Interactable>();

                    if (interactable != null)
                    {
                        SetFocus(interactable);
                    }




                    //_agent.SetDestination(hit.point);
                }
            }

            if (_target != null)
            {
                _agent.SetDestination(_target.position);
            }
        }

        private void SetFocus(Interactable newFocus)
        {
            if (newFocus != _focus)
            {
                if (_focus != null)
                {
                    _focus.OnDefocused();
                }

                _focus = newFocus;

                FollowTarget(newFocus);
            }

            newFocus.OnFocused(transform);
        }

        private void RemoveFocus()
        {
            if (_focus != null)
            {
                _focus.OnDefocused();
            }

            _focus = null;

            StopFollowingTarget();
        }

        private void FollowTarget(Interactable newTarget)
        {
            _agent.stoppingDistance = newTarget.Radius * .8f;
            _agent.updateRotation = false;

            _target = newTarget.transform;
        }

        private void StopFollowingTarget()
        {
            _agent.updateRotation = true;
            _target = null;
        }

        private void FaceTarget()
        {
            var direction = (_target.position - transform.position).normalized;

            var lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }

        private void Test()
        {
            float speed = 10;

            if (speed < 20)
            {
                // Stay alert
            }
            else if (speed <= 20 && speed < 30)
            {
                bool peopleCrossing = true;

                if (peopleCrossing)
                {
                    // Stop
                }
                else
                {
                    // GO
                }
            }
            else if (speed <= 30 && speed <= 70)
            {
                string weather = "R";

                if (weather == "R")
                {
                    // 4 second gap
                }
                else
                {
                    // 2 second gap
                }
            }


        }
    }
}