using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(HealthComponent))]
    [RequireComponent(typeof(GroundedComponent))]
    [RequireComponent(typeof(JumpComponent))]
    [RequireComponent(typeof(MoveComponent))]
    [RequireComponent(typeof(LookComponent))]
    [RequireComponent(typeof(ExtraGravityComponent))]
    public class Character : MonoBehaviour, IMoveComponent, IJumpComponent, IFireComponent
    {
        [SerializeField] private GameObject _pushAttack;
        [SerializeField] private GameObject _tossAttack;
        
        private JumpComponent _jumpComponent;
        private HealthComponent _healthComponent;
        private GroundedComponent _groundedComponent;
        private ExtraGravityComponent _extraGravityComponent;
        private MoveComponent _moveComponent;
        private LookComponent _lookComponent;
        private FireComponent _pushComponent;
        private FireComponent _tossComponent;
        private Rigidbody2D _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _healthComponent = GetComponent<HealthComponent>();
            _groundedComponent = GetComponent<GroundedComponent>();
            _extraGravityComponent = GetComponent<ExtraGravityComponent>();
            _jumpComponent = GetComponent<JumpComponent>();
            _moveComponent = GetComponent<MoveComponent>();
            _lookComponent = GetComponent<LookComponent>();
            _pushComponent = _pushAttack.GetComponent<FireComponent>();
            _tossComponent = _tossAttack.GetComponent<FireComponent>();

            _jumpComponent.SetCondition(() => _healthComponent.IsAlive && 
                                              _groundedComponent.IsGrounded);
            
            _moveComponent.SetCondition(() => _healthComponent.IsAlive);
            _pushComponent.SetCondition(() => _healthComponent.IsAlive);
            _tossComponent.SetCondition(() => _healthComponent.IsAlive);

            _healthComponent.OnDied += OnDied;
        }

        private void OnDestroy() => 
            _healthComponent.OnDied += OnDied;

        private void Update() => 
            _extraGravityComponent.enabled = _rigidbody.linearVelocityY < 0;

        public void Move(Vector2 direction)
        {
            _lookComponent.Look(direction.x);
            _moveComponent.Move(new Vector2(Mathf.Abs(direction.x), direction.y));
        }

        public void Jump() => 
            _jumpComponent.Jump();

        public void Push() => 
            _pushComponent.Fire();

        public void Toss() => 
            _tossComponent.Fire();

        private void OnDied() => 
            _rigidbody.simulated = false;
    }
}