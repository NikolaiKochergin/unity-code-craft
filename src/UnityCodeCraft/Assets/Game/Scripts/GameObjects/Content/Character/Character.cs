using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(HealthComponent))]
    [RequireComponent(typeof(GroundedComponent))]
    [RequireComponent(typeof(JumpComponent))]
    [RequireComponent(typeof(MoveComponent))]
    [RequireComponent(typeof(MoveTransformComponent))]
    [RequireComponent(typeof(LookComponent))]
    public sealed class Character : MonoBehaviour, IPushComponent, ITossComponent
    {
        [SerializeField] private ForceComponent _pushComponent;
        [SerializeField] private ForceComponent _tossComponent;
        
        private JumpComponent _jumpComponent;
        private HealthComponent _healthComponent;
        private GroundedComponent _groundedComponent;
        private MoveComponent _moveComponent;
        private MoveTransformComponent _moveTransformComponent;
        private LookComponent _lookComponent;
        private Rigidbody2D _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _healthComponent = GetComponent<HealthComponent>();
            _groundedComponent = GetComponent<GroundedComponent>();
            _jumpComponent = GetComponent<JumpComponent>();
            _moveComponent = GetComponent<MoveComponent>();
            _moveTransformComponent = GetComponent<MoveTransformComponent>();
            _lookComponent = GetComponent<LookComponent>();

            _jumpComponent.SetCondition(() => _healthComponent.IsAlive && 
                                              _groundedComponent.IsGrounded);
            
            _moveComponent.SetCondition(() => _healthComponent.IsAlive);
            _moveComponent.SetAction((direction, deltaTime) =>
            {
                _lookComponent.Look(direction.x);
                _moveTransformComponent.Move(new Vector2(Mathf.Abs(direction.x), direction.y), deltaTime);
            });
            
            _pushComponent.SetCondition(() => _healthComponent.IsAlive);
            _tossComponent.SetCondition(() => _healthComponent.IsAlive);

            _healthComponent.OnDied += OnDied;
        }

        private void OnDestroy() => 
            _healthComponent.OnDied += OnDied;

        public void Push() => 
            _pushComponent.Apply();

        public void Toss() => 
            _tossComponent.Apply();

        private void OnDied() => 
            _rigidbody.simulated = false;
    }
}