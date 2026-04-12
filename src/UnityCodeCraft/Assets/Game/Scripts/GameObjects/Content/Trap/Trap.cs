using Game.Scripts.GameObjects.Components.Death;
using UnityEngine;

namespace Game.Scripts.GameObjects.Content.Trap
{
    public class Trap : MonoBehaviour,
        DeathHandleComponent.IAction
    {
        [SerializeField] private int _damage = 1;
        
        private CollisionComponent _collisionComponent;
        private HealthComponent _healthComponent;
        private DeathHandleComponent _deathHandleComponent;

        private void Awake()
        {
            _healthComponent = GetComponent<HealthComponent>();
            _deathHandleComponent = GetComponent<DeathHandleComponent>();
            _deathHandleComponent.SetAction(this);

            _collisionComponent = GetComponent<CollisionComponent>();

            _collisionComponent.OnEntered += OnEntered;
        }

        private void OnDestroy()
        {
            _collisionComponent.OnEntered -= OnEntered;
        }

        private void OnEntered(Collision2D collision)
        {
            if(!collision.collider.TryGetComponent(out HealthComponent healthComponent))
                return;
            
            healthComponent.TakeDamage(_damage);
            Destroy(gameObject);
        }

        void DeathHandleComponent.IAction.Invoke()
        {
            if(!_healthComponent.IsAlive)
                Destroy(gameObject);
        }
    }
}
