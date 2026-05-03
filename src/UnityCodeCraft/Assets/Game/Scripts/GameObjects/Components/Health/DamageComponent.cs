using UnityEngine;

namespace Game
{
    public sealed class DamageComponent : MonoBehaviour
    {
        [SerializeField, Min(0)] private float _damage;
        
        public bool DealDamage(GameObject target)
        {
            if (!(_damage > 0) || !target.TryGetComponent(out HealthComponent health)) 
                return false;
            
            health.TakeDamage(_damage);
            return true;
        }
    }
}