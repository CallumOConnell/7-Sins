using Sins.Character;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Sins.AI
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField]
        private float _detectionRange = 10f;

        [SerializeField]
        private float _patrolRange = 5f;

        [SerializeField]
        private float _stunDuration = 1f;

        [SerializeField]
        private LayerMask _groundLayer;

        private bool _targetInSightRange = false, _targetInAttackRange = false, _isTravelling = false, _isWaiting = false, _isStunned = false, _isDistracted = false;

        private float _waitTimer = 0f, _timeToWait = 5f;

        private Vector3 _startPoint;
        
        private NavMeshAgent _agent;

        private Transform _target;

        private EnemyCombat _combat;

        public void Distract(GameObject bait)
        {
            _isDistracted = true;

            while (bait != null)
            {
                _agent.SetDestination(bait.transform.position);
            }

            _isDistracted = false;
        }

        // Debug for seeing the enemies detection radius
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _detectionRange);
        }

        private void Start()
        {
            _target = Player.Instance.transform;
            _agent = GetComponent<NavMeshAgent>();
            _combat = GetComponent<EnemyCombat>();

            _startPoint = transform.position;
        }

        private void Update()
        {
            var distance = Vector3.Distance(_target.position, transform.position);

            _targetInSightRange = distance <= _detectionRange;
            _targetInAttackRange = distance <= _agent.stoppingDistance;

            if (!_targetInSightRange && !_targetInAttackRange && !_isStunned && !_isDistracted)
            {
                Patrol();
            }

            if (_targetInSightRange && !_targetInAttackRange && !_isStunned && !_isDistracted)
            {
                ChaseTarget();
            }

            if (_targetInSightRange && _targetInAttackRange && !_isStunned && !_isDistracted)
            {
                AttackTarget();
            }
        }

        private void Patrol()
        {
            Debug.Log("A");

            if (!_isTravelling && !_isWaiting)
            {
                Debug.Log("B");
                FindPatrolPoint();
            }

            if (_isTravelling && _agent.remainingDistance <= 2f)
            {
                Debug.Log("C");
                _isTravelling = false;

                _isWaiting = true;

                _waitTimer = 0f;
            }

            if (_isWaiting)
            {
                Debug.Log("D");
                _waitTimer += Time.deltaTime;

                if (_waitTimer >= _timeToWait)
                {
                    Debug.Log("E");
                    _isWaiting = false;

                    FindPatrolPoint();
                }
            }
        }

        private void FindPatrolPoint()
        {
            Debug.Log("F");
            var randomZ = Random.Range(-_patrolRange, _patrolRange);
            var randomX = Random.Range(-_patrolRange, _patrolRange);

            var patrolPoint = _startPoint + new Vector3(randomX, 0, randomZ);

            if (Physics.Raycast(patrolPoint, -transform.up, float.MaxValue, _groundLayer))
            {
                Debug.Log("G");
                _isTravelling = true;

                _agent.SetDestination(patrolPoint);
            }
        }

        private void ChaseTarget() => _agent.SetDestination(_target.position);

        private void AttackTarget()
        {
            FaceTarget();

            _agent.isStopped = true;

            var targetStats = _target.GetComponent<CharacterStats>();

            if (targetStats != null)
            {
                _combat.Attack(targetStats);

                _agent.isStopped = false;
            }
        }

        private void FaceTarget()
        {
            var direction = (_target.position - transform.position).normalized;

            var lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }

        private IEnumerator StunCooldown()
        {
            yield return new WaitForSeconds(_stunDuration);

            _isStunned = false;
        }

        public void Stun()
        {
            _isStunned = true;

            StartCoroutine(StunCooldown());

            // Start some effect here to show enemy is stunned?
        }
    }
}