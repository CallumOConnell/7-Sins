using UnityEngine;

namespace Sins.Abilities
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField]
        private float _lifeTime = 5f;
        
        public bool UseImpactEffect = false;

        public int Damage { get; set; }

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