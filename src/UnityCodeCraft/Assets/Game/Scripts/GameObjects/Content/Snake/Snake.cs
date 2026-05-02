using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(HealthComponent))]
    [RequireComponent(typeof(MoveComponent))]
    [RequireComponent(typeof(LookComponent))]
    [RequireComponent(typeof(CollisionComponent))]
    public class Snake : MonoBehaviour
    {
        [SerializeField] private ForceComponent _tossComponent;
        [SerializeField] private GameObject _target;
        
        private Rigidbody2D _rigidbody;
        private HealthComponent _healthComponent;
        private MoveComponent _moveComponent;
        private LookComponent _lookComponent;
        private CollisionComponent _collisionComponent;
        private TriggerComponent _characterTriggerComponent;
        private DamageComponent _damageComponent;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _healthComponent = GetComponent<HealthComponent>();
            _moveComponent = GetComponent<MoveComponent>();
            _lookComponent = GetComponent<LookComponent>();
            _collisionComponent = GetComponent<CollisionComponent>();
            _characterTriggerComponent = GetComponentInChildren<TriggerComponent>();
            _damageComponent = _tossComponent.GetComponent<DamageComponent>();

            _moveComponent.SetCondition(() => _healthComponent.IsAlive);
            _tossComponent.SetCondition(() => _healthComponent.IsAlive);
            _tossComponent.SetAction(targets =>
            {
                foreach (GameObject target in targets) 
                    _damageComponent.DealDamage(target);
            });
            
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
                _target = col.gameObject;
        }

        private void OnCharacterExited(Collider2D col)
        {
            if(col.CompareTag(GameObjectTags.Character))
                _target = null;
        }

        private void Update()
        {
            if (!_target) 
                return;
            
            Vector2 direction = (_target.transform.position - transform.position).normalized;
            _lookComponent.Look(_target.transform);
            _moveComponent.Move(new Vector2(Mathf.Abs(direction.x), 0));
        }

        private void OnCollisionEntered(Collision2D col)
        {
            if (col.gameObject.CompareTag(GameObjectTags.Character))
                _tossComponent.Fire();
        }

        private void OnDied() => 
            _rigidbody.simulated = false;
    }
}