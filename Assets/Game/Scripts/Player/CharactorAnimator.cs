using UnityEngine;
using UnityEngine.AI;

namespace Sins.Character
{
    public class CharactorAnimator : MonoBehaviour
    {
        [SerializeField]
        private Animator _animator;

        [SerializeField]
        private float _locomationAnimationSmoothTime = 0.02f;

        private NavMeshAgent _agent;

        private void Awake() => _agent = GetComponent<NavMeshAgent>();

        private void Update()
        {
            var speedPercentage = _agent.velocity.magnitude / _agent.speed;

            _animator.SetFloat("speedPercentage", speedPercentage, _locomationAnimationSmoothTime, Time.deltaTime);
        }
    }
}