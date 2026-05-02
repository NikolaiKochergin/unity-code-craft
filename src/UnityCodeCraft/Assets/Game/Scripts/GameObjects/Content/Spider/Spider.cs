using UnityEngine;

namespace Game.Scripts.GameObjects.Content.Spider
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(HealthComponent))]
    [RequireComponent(typeof(GroundedComponent))]
    [RequireComponent(typeof(WaypointMoveComponent))]
    [RequireComponent(typeof(CollisionComponent))]
    public class Spider : MonoBehaviour
    {
        [SerializeField] private GameObject _pushAttack;
        
        private Rigidbody2D _rigidbody;
        private HealthComponent _healthComponent;
        private GroundedComponent _groundedComponent;
        private WaypointMoveComponent _waypointMoveComponent;
        private ForceComponent _pushComponent;
        private CollisionComponent _collisionComponent;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _healthComponent = GetComponent<HealthComponent>();
            _groundedComponent = GetComponent<GroundedComponent>();
            _waypointMoveComponent = GetComponent<WaypointMoveComponent>();
            _collisionComponent = GetComponent<CollisionComponent>();
            _pushComponent = _pushAttack.GetComponent<ForceComponent>();
            
            _waypointMoveComponent.SetCondition(() => _healthComponent.IsAlive);
            _pushComponent.SetCondition(() => _healthComponent.IsAlive &&
                                              _groundedComponent.IsGrounded);

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
                _pushComponent.Fire();
        }

        private void OnDied() => 
            _rigidbody.simulated = false;
    }
}