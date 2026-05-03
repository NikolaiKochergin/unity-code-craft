using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(HealthComponent))]
    [RequireComponent(typeof(GroundedComponent))]
    [RequireComponent(typeof(WaypointMoveComponent))]
    [RequireComponent(typeof(MoveTransformComponent))]
    [RequireComponent(typeof(CollisionComponent))]
    public sealed class Spider : MonoBehaviour
    {
        [SerializeField] private ForceComponent _pushComponent;
        
        private Rigidbody2D _rigidbody;
        private HealthComponent _healthComponent;
        private GroundedComponent _groundedComponent;
        private WaypointMoveComponent _waypointMoveComponent;
        private MoveTransformComponent _moveTransformComponent;
        private CollisionComponent _collisionComponent;
        private DamageComponent _damageComponent;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _healthComponent = GetComponent<HealthComponent>();
            _groundedComponent = GetComponent<GroundedComponent>();
            _waypointMoveComponent = GetComponent<WaypointMoveComponent>();
            _moveTransformComponent = GetComponent<MoveTransformComponent>();
            _collisionComponent = GetComponent<CollisionComponent>();
            _damageComponent = _pushComponent.GetComponent<DamageComponent>();
            
            _waypointMoveComponent.SetCondition(() => _healthComponent.IsAlive);
            _waypointMoveComponent.SetAction(_moveTransformComponent.Move);
            
            _pushComponent.SetCondition(() => _healthComponent.IsAlive &&
                                              _groundedComponent.IsGrounded);
            
            _pushComponent.SetAction(target => _damageComponent.DealDamage(target));

            _healthComponent.OnDied += OnDied;
            _collisionComponent.OnEntered += OnCollisionEntered;
        }

        private void OnDestroy()
        {
            _healthComponent.OnDied -= OnDied;
            _collisionComponent.OnEntered -= OnCollisionEntered;
        }
        
        private void OnCollisionEntered(Collision2D col)
        {
            if (col.gameObject.CompareTag(GameObjectTags.Character))
                _pushComponent.Apply();
        }

        private void OnDied() => 
            _rigidbody.simulated = false;
    }
}