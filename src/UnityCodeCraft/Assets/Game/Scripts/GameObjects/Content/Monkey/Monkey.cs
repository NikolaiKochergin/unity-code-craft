using UnityEngine;

namespace Game.Scripts.GameObjects.Content.Monkey
{
    public class Monkey : MonoBehaviour,
        DamageRequestComponent.ICondition,
        DamageRequestComponent.IAction
    {
        [SerializeField] private LookComponent _lookComponent;
        [SerializeField] private ForceAbility _pushAbility;
        [SerializeField] private JumpAbility _jumpAbility;
        [SerializeField] private GroundedComponent _groundedComponent;
        [SerializeField] private TriggerComponent _triggerComponent;
        [SerializeField] private CollisionComponent _collisionComponent;
        [SerializeField] private DamageRequestComponent _damageRequest;
        [SerializeField] private DamageComponent _damageComponent;
        
        private HealthComponent _healthComponent;
        private GameObject _target;
        
        private void Awake()
        {
            _healthComponent = GetComponent<HealthComponent>();
            
            _damageRequest.SetCondition(this);
            _damageRequest.SetAction(this);
            
            _healthComponent.OnDied += OnDied;
            _groundedComponent.OnGrounded += OnGrounded;
            _triggerComponent.OnEntered += OnCharacterEnter;
            _triggerComponent.OnExited += OnCharacterExit;
            _collisionComponent.OnEntered += OnCollisionEntered;
        }
        
        private void OnDestroy()
        {
            _healthComponent.OnDied -= OnDied;
            _groundedComponent.OnGrounded -= OnGrounded;
            _triggerComponent.OnEntered -= OnCharacterEnter;
            _triggerComponent.OnExited -= OnCharacterExit;
            _collisionComponent.OnEntered -= OnCollisionEntered;
        }

        private void OnCollisionEntered(Collision2D collision) => 
            _damageRequest.Damage(collision.gameObject);

        private void OnCharacterExit(Collider2D col)
        {
            if(col.gameObject.layer == LayerMask.NameToLayer("Character"))
                _target = col.gameObject;
        }

        private void OnCharacterEnter(Collider2D col)
        {
            if(col.gameObject.layer == LayerMask.NameToLayer("Character"))
                _target = null;
        }

        private void Update()
        {
            if(_healthComponent.IsDied)
                return;
            
            _jumpAbility.Jump();
            if(_target)
                _lookComponent.Look(_target.transform);
        }

        private void OnDied() => 
            GetComponent<Rigidbody2D>().simulated = false;

        private void OnGrounded(bool isGrounded)
        {
            if(_healthComponent.IsAlive)
                _pushAbility.Apply();
        }
        
        bool DamageRequestComponent.ICondition.Evaluate() => 
            _healthComponent.IsAlive;

        void DamageRequestComponent.IAction.Invoke(HealthComponent health) => 
            _damageComponent.Apply(health);
    }
}