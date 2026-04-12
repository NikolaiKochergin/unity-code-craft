using Game.Scripts.GameObjects.Components.Death;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(MoveRequestComponent), typeof(HealthComponent))]
    public class Character : MonoBehaviour,
        MoveRequestComponent.ICondition,
        MoveRequestComponent.IAction,
        JumpRequestComponent.ICondition,
        JumpRequestComponent.IAction,
        DeathHandleComponent.IAction,
        FallingHandleComponent.IAction,
        IPushComponent,
        ITossComponent
    {
        [SerializeField] private GameObject _abilities;
        
        private HealthComponent _healthComponent;
        private DeathHandleComponent _deathHandleComponent;
        private MoveRequestComponent _moveRequestComponent;
        private MoveTransformComponent _moveComponent;
        private LookComponent _lookComponent;
        
        private JumpRequestComponent _jumpRequestComponent;
        private JumpComponent _jumpComponent;
        private GroundedComponent _groundedComponent;
        private ExtraGravityComponent _extraGravityComponent;
        private FallingHandleComponent _fallingHandleComponent;

        private void Awake()
        {
            _healthComponent = GetComponent<HealthComponent>();
            _deathHandleComponent = GetComponent<DeathHandleComponent>();
            _deathHandleComponent.SetAction(this);
            
            _moveRequestComponent = GetComponent<MoveRequestComponent>();
            _moveComponent = GetComponent<MoveTransformComponent>();
            _lookComponent = GetComponent<LookComponent>();
            
            _moveRequestComponent.SetAction(this);
            _moveRequestComponent.SetCondition(this);
            
            _jumpRequestComponent = GetComponent<JumpRequestComponent>();
            _jumpComponent = GetComponent<JumpComponent>();
            _groundedComponent = GetComponent<GroundedComponent>();
            _extraGravityComponent = GetComponent<ExtraGravityComponent>();
            _fallingHandleComponent = GetComponent<FallingHandleComponent>();
            
            _fallingHandleComponent.SetAction(this);
            _jumpRequestComponent.SetAction(this);
            _jumpRequestComponent.SetCondition(this);
        }

        public void Push()
        {
            if (_healthComponent.IsAlive)
                _abilities.GetComponentInChildren<PushRequestComponent>()?.Push();
        }

        public void Toss()
        {
            if (_healthComponent.IsAlive)
                _abilities.GetComponentInChildren<TossRequestComponent>()?.Toss();
        }

        bool MoveRequestComponent.ICondition.Evaluate() =>
            _healthComponent.IsAlive;

        void MoveRequestComponent.IAction.Invoke(Vector2 direction)
        {
            _lookComponent.Look(direction.x);
            _moveComponent.Move(new Vector2(Mathf.Abs(direction.x), direction.y));
        }

        bool JumpRequestComponent.ICondition.Evaluate() => 
            _healthComponent.IsAlive &&
            _groundedComponent.IsGrounded;

        void JumpRequestComponent.IAction.Invoke() => 
            _jumpComponent.Jump();

        void DeathHandleComponent.IAction.Invoke() => 
            GetComponent<Rigidbody2D>()!.simulated = false;
        
        void FallingHandleComponent.IAction.Invoke(bool isFalling) =>
            _extraGravityComponent.enabled = isFalling;
    }
}
