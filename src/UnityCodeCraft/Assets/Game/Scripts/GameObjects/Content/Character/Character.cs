using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(HealthComponent))]
    [RequireComponent(typeof(GroundedComponent))]
    [RequireComponent(typeof(JumpComponent))]
    [RequireComponent(typeof(MoveComponent))]
    [RequireComponent(typeof(LookComponent))]
    public class Character : MonoBehaviour, IMoveComponent, IJumpComponent, IPushComponent, ITossComponent
    {
        [SerializeField] private ForceComponent _pushComponent;
        [SerializeField] private ForceComponent _tossComponent;
        
        private JumpComponent _jumpComponent;
        private HealthComponent _healthComponent;
        private GroundedComponent _groundedComponent;
        private MoveComponent _moveComponent;
        private LookComponent _lookComponent;
        private Rigidbody2D _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _healthComponent = GetComponent<HealthComponent>();
            _groundedComponent = GetComponent<GroundedComponent>();
            _jumpComponent = GetComponent<JumpComponent>();
            _moveComponent = GetComponent<MoveComponent>();
            _lookComponent = GetComponent<LookComponent>();

            _jumpComponent.SetCondition(() => _healthComponent.IsAlive && 
                                              _groundedComponent.IsGrounded);
            
            _moveComponent.SetCondition(() => _healthComponent.IsAlive);
            _pushComponent.SetCondition(() => _healthComponent.IsAlive);
            _tossComponent.SetCondition(() => _healthComponent.IsAlive);

            _healthComponent.OnDied += OnDied;
        }

        private void OnDestroy() => 
            _healthComponent.OnDied += OnDied;

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