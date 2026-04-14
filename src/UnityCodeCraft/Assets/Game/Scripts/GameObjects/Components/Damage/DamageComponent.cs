using UnityEngine;

namespace Game
{
    public class DamageComponent : MonoBehaviour
    {
        [SerializeField] private int _damage = 1;

        public void Apply(HealthComponent target) => 
            target.TakeDamage(_damage);
    }
}