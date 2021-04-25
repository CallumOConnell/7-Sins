using UnityEngine;
using UnityEngine.AI;
using Sins.Interaction;

namespace Sins.Character
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private Camera _camera;

        [SerializeField]
        private LayerMask _groundLayer;

        private Transform _target;

        private NavMeshAgent _agent;

        private Interactable _focus;

        public bool MovementLocked { get; set; }

        private void Awake() => _agent = GetComponent<NavMeshAgent>();

        private void Update()
        {
            if (!MovementLocked)
            {
                if (Input.GetMouseButton(0)) // Left mouse button
                {
                    var ray = _camera.ScreenPointToRay(Input.mousePosition);

                    if (Physics.Raycast(ray, out RaycastHit hit, 100, _groundLayer))
                    {
                        _agent.SetDestination(hit.point);

                        RemoveFocus();
                    }
                }

                if (Input.GetMouseButtonDown(1)) // Right mouse button
                {
                    var ray = _camera.ScreenPointToRay(Input.mousePosition);

                    if (Physics.Raycast(ray, out RaycastHit hit, 100))
                    {
                        Interactable interactable = hit.collider.GetComponent<Interactable>();

                        if (interactable != null)
                        {
                            SetFocus(interactable);
                        }

                        _agent.SetDestination(hit.point);
                    }
                }

                if (_target != null)
                {
                    _agent.SetDestination(_target.position);

                    FaceTarget();
                }
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
            _agent.stoppingDistance = newTarget.Radius * 0.8f;
            _agent.updateRotation = false;

            _target = newTarget.transform;
        }

        private void StopFollowingTarget()
        {
            _agent.stoppingDistance = 0f;
            _agent.updateRotation = true;

            _target = null;
        }

        private void FaceTarget()
        {
            var direction = (_target.position - transform.position).normalized;

            var lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));

            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }
}