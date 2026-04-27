using UnityEngine;

namespace Game.Scripts.GameObjects.Content.Trap
{
    [RequireComponent(typeof(HealthComponent))]
    [RequireComponent(typeof(CollisionComponent))]
    public class Trap : MonoBehaviour
    {
        [SerializeField, Min(0)] private float _damage;
        
        private HealthComponent _healthComponent;
        private CollisionComponent _collisionComponent;

        private void Awake()
        {
            _healthComponent = GetComponent<HealthComponent>();
            _collisionComponent = GetComponent<CollisionComponent>();

            _healthComponent.OnDied += OnDie;
            _collisionComponent.OnEntered += OnEntered;
        }

        private void OnDestroy()
        {
            _healthComponent.OnDied -= OnDie;
            _collisionComponent.OnEntered -= OnEntered;
        }

        private void OnEntered(Collision2D col)
        {
            if (!(_damage > 0) || !col.gameObject.TryGetComponent(out HealthComponent health)) 
                return;
            
            health.TakeDamage(_damage);
            OnDie();
        }

        private void OnDie() => 
            Destroy(gameObject);
    }
}