using UnityEngine;

namespace Sins.Abilities
{
    public class PoisonCloud : MonoBehaviour
    {
        [SerializeField]
        private float _lifeTime = 5f;

        private void Update()
        {
            _lifeTime -= Time.deltaTime;

            if (_lifeTime <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}