using UnityEngine;

namespace Sins.Interaction
{
    public class Interactable : MonoBehaviour
    {
        [SerializeField]
        private float _radius = 3f;

        private bool isFocus = false, hasInteracted = false;

        private Transform player;

        public float Radius { get => _radius; set => _radius = value; }

        private void OnDrawGizmosSelected() // Debug for displaying the radius around an interactable
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _radius);
        }

        public virtual void Update()
        {
            if (isFocus && !hasInteracted)
            {
                float distance = Vector3.Distance(player.position, transform.position);

                if (distance <= _radius)
                {
                    Interact();

                    hasInteracted = true;
                }
            }
        }

        public virtual void Interact()
        {
            Debug.Log($"Interacted with {transform.name}");
        }

        public void OnFocused(Transform playerTransform)
        {
            isFocus = true;
            player = playerTransform;
            hasInteracted = false;
        }

        public void OnDefocused()
        {
            isFocus = false;
            player = null;
            hasInteracted = false;
        }
    }
}