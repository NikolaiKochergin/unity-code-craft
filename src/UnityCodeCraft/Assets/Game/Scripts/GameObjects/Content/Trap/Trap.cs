using UnityEngine;

namespace Game.Scripts.GameObjects.Content.Trap
{
    [RequireComponent(typeof(HealthComponent))]
    [RequireComponent(typeof(CollisionComponent))]
    [RequireComponent(typeof(DamageComponent))]
    public class Trap : MonoBehaviour
    {
        private HealthComponent _healthComponent;
        private CollisionComponent _collisionComponent;
        private DamageComponent _damageComponent;

        private void Awake()
        {
            _healthComponent = GetComponent<HealthComponent>();
            _collisionComponent = GetComponent<CollisionComponent>();
            _damageComponent = GetComponent<DamageComponent>();

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
            if(_damageComponent.DealDamage(col.gameObject))
                OnDie();
        }

        private void OnDie() => 
            Destroy(gameObject);
    }
}