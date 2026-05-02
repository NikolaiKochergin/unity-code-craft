using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(HealthComponent))]
    [RequireComponent(typeof(GroundedComponent))]
    [RequireComponent(typeof(JumpComponent))]
    [RequireComponent(typeof(LookComponent))]
    [RequireComponent(typeof(CollisionComponent))]
    public class Monkey : MonoBehaviour
    {
        [SerializeField] private GameObject _pushAttack;
        [SerializeField] private GameObject _target;
        [SerializeField, Min(0)] private float _damage;
        
        private Rigidbody2D _rigidbody;
        private HealthComponent _healthComponent;
        private JumpComponent _jumpComponent;
        private LookComponent _lookComponent;
        private CollisionComponent _collisionComponent;
        private FireComponent _pushComponent;
        private TriggerComponent _characterTriggerComponent;
        private GroundedComponent _groundedComponent;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _healthComponent = GetComponent<HealthComponent>();
            _groundedComponent = GetComponent<GroundedComponent>();
            _jumpComponent = GetComponent<JumpComponent>();
            _lookComponent = GetComponent<LookComponent>();
            _collisionComponent = GetComponent<CollisionComponent>();
            _pushComponent = _pushAttack.GetComponent<FireComponent>();
            _characterTriggerComponent = GetComponentInChildren<TriggerComponent>();

            _pushComponent.SetCondition(() => _healthComponent.IsAlive);
            _jumpComponent.SetCondition(() => _healthComponent.IsAlive &&
                                              _groundedComponent.IsGrounded);
            
            _healthComponent.OnDied += OnDied;
            _collisionComponent.OnEntered += OnCollisionEntered;

            _characterTriggerComponent.OnEntered += OnCharacterEntered;
            _characterTriggerComponent.OnExited += OnCharacterExited;

            _groundedComponent.OnGrounded += OnGrounded;
        }

        private void OnDestroy()
        {
            _healthComponent.OnDied -= OnDied;
            _collisionComponent.OnEntered -= OnCollisionEntered;

            _characterTriggerComponent.OnEntered -= OnCharacterEntered;
            _characterTriggerComponent.OnExited -= OnCharacterExited;
            
            _groundedComponent.OnGrounded -= OnGrounded;
        }

        private void Update()
        {
            _jumpComponent.Jump();
            
            if(_target)
                _lookComponent.Look(_target.transform);
        }

        private void OnGrounded(bool _) => 
            _pushComponent.Fire();

        private void OnCharacterEntered(Collider2D col)
        {
            if(col.gameObject.layer == LayerMask.NameToLayer(GameObjectTags.Character))
                _target = col.gameObject;
        }

        private void OnCharacterExited(Collider2D col)
        {
            if(col.gameObject.layer == LayerMask.NameToLayer(GameObjectTags.Character))
                _target = null;
        }

        private void OnCollisionEntered(Collision2D col)
        {
            if(_damage > 0 && col.gameObject.TryGetComponent(out HealthComponent health))
                health.TakeDamage(_damage);
        }

        private void OnDied() => 
            _rigidbody.simulated = false;
    }
}