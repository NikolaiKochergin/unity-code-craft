using UnityEngine;

namespace Game.Scripts.GameObjects.Content.Trap
{
    public class Trap : MonoBehaviour,
        DamageRequestComponent.ICondition,
        DamageRequestComponent.IAction
    {
        private HealthComponent _healthComponent;
        private CollisionComponent _collisionComponent;
        private DamageComponent _damageComponent;
        private DamageRequestComponent _damageRequest;

        private void Awake()
        {
            _healthComponent = GetComponent<HealthComponent>();
            _collisionComponent = GetComponent<CollisionComponent>();

            _damageRequest = GetComponentInChildren<DamageRequestComponent>();
            _damageComponent = GetComponentInChildren<DamageComponent>();
            
            _damageRequest.SetCondition(this);
            _damageRequest.SetAction(this);

            _collisionComponent.OnEntered += OnEntered;
            _healthComponent.OnDied += OnDied;
        }

        private void OnDestroy()
        {
            _healthComponent.OnDied -= OnDied;
            _collisionComponent.OnEntered -= OnEntered;
        }

        private void OnEntered(Collision2D collision) => 
            _damageRequest.Damage(collision.gameObject);

        private void OnDied()
        {
            if(!_healthComponent.IsAlive)
                Destroy(gameObject);
        }

        bool DamageRequestComponent.ICondition.Evaluate() => 
            _healthComponent.IsAlive;

        void DamageRequestComponent.IAction.Invoke(HealthComponent health)
        {
            _damageComponent.Apply(health);
            Destroy(gameObject);
        }
    }
}
