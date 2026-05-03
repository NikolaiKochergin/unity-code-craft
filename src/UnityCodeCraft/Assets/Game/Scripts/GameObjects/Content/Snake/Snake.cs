using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(HealthComponent))]
    [RequireComponent(typeof(ChaseMoveComponent))]
    [RequireComponent(typeof(MoveTransformComponent))]
    [RequireComponent(typeof(TargetComponent))]
    [RequireComponent(typeof(LookComponent))]
    [RequireComponent(typeof(CollisionComponent))]
    public sealed class Snake : MonoBehaviour
    {
        [SerializeField] private ForceComponent _tossComponent;
        
        private Rigidbody2D _rigidbody;
        private HealthComponent _healthComponent;
        private ChaseMoveComponent _chaseMoveComponent;
        private MoveTransformComponent _moveTransformComponent;
        private TargetComponent _targetComponent;
        private LookComponent _lookComponent;
        private CollisionComponent _collisionComponent;
        private TriggerComponent _characterTriggerComponent;
        private DamageComponent _damageComponent;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _healthComponent = GetComponent<HealthComponent>();
            _chaseMoveComponent = GetComponent<ChaseMoveComponent>();
            _moveTransformComponent = GetComponent<MoveTransformComponent>();
            _targetComponent = GetComponent<TargetComponent>();
            _lookComponent = GetComponent<LookComponent>();
            _collisionComponent = GetComponent<CollisionComponent>();
            _characterTriggerComponent = GetComponentInChildren<TriggerComponent>();
            _damageComponent = _tossComponent.GetComponent<DamageComponent>();

            _chaseMoveComponent.SetCondition(() => _healthComponent.IsAlive);
            _chaseMoveComponent.SetAction(direction =>
            {
                _lookComponent.Look(direction.x);
                _moveTransformComponent.Move(new Vector2(Mathf.Abs(direction.x), direction.y));
            });
            
            _tossComponent.SetCondition(() => _healthComponent.IsAlive);
            _tossComponent.SetAction(target => _damageComponent.DealDamage(target));
            
            _healthComponent.OnDied += OnDied;
            _collisionComponent.OnEntered += OnCollisionEntered;

            _characterTriggerComponent.OnEntered += OnCharacterEntered;
            _characterTriggerComponent.OnExited += OnCharacterExited;
        }
        
        private void OnDestroy()
        {
            _healthComponent.OnDied -= OnDied;
            _collisionComponent.OnEntered -= OnCollisionEntered;
            _characterTriggerComponent.OnEntered -= OnCharacterEntered;
            _characterTriggerComponent.OnExited -= OnCharacterExited;
        }

        private void OnCharacterEntered(Collider2D col)
        {
            if(col.CompareTag(GameObjectTags.Character))
                _targetComponent.Target = col.gameObject; }

        private void OnCharacterExited(Collider2D col)
        {
            if(col.CompareTag(GameObjectTags.Character))
                _targetComponent.Target = null;
        }

        private void OnCollisionEntered(Collision2D col)
        {
            if (col.gameObject.CompareTag(GameObjectTags.Character))
                _tossComponent.Apply();
        }

        private void OnDied() => 
            _rigidbody.simulated = false;
    }
}