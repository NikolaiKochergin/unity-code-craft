using UnityEngine;

namespace Game.Scripts.GameObjects.Content.Trap
{
    public class Trap : MonoBehaviour
    {
        [SerializeField] private int _damage = 1;
        
        private CollisionComponent _collisionComponent;
        private HealthComponent _healthComponent;

        private void Awake()
        {
            _healthComponent = GetComponent<HealthComponent>();
            _collisionComponent = GetComponent<CollisionComponent>();

            _collisionComponent.OnEntered += OnEntered;
            _healthComponent.OnDied += OnDied;
        }

        private void OnDestroy()
        {
            _healthComponent.OnDied -= OnDied;
            _collisionComponent.OnEntered -= OnEntered;
        }

        private void OnEntered(Collision2D collision)
        {
            if(!collision.collider.TryGetComponent(out HealthComponent healthComponent))
                return;
            
            healthComponent.TakeDamage(_damage);
            Destroy(gameObject);
        }

        private void OnDied()
        {
            if(!_healthComponent.IsAlive)
                Destroy(gameObject);
        }
    }
}
