using UnityEngine;

namespace Game.Scripts.GameObjects.Content.Snake
{
    public class Snake : MonoBehaviour,
        MoveRequestComponent.ICondition,
        MoveRequestComponent.IAction,
        DamageRequestComponent.ICondition,
        DamageRequestComponent.IAction
    {
        [SerializeField] private GameObject _target;
        [SerializeField] private TossAbility _tossAbility;
        [SerializeField] private GameObject _damage;
        
        private HealthComponent _healthComponent;
        private MoveRequestComponent _moveRequestComponent;
        private MoveTransformComponent _moveComponent;
        private LookComponent _lookComponent;
        private TriggerComponent _characterTrigger;
        private CollisionComponent _collisionComponent;
        private DamageRequestComponent _damageRequest;
        private DamageComponent _damageComponent;
        private CooldownComponent _cooldownComponent;

        private void Awake()
        {
            _healthComponent = GetComponent<HealthComponent>();
            
            _moveRequestComponent = GetComponent<MoveRequestComponent>();
            _moveComponent = GetComponent<MoveTransformComponent>();
            _lookComponent = GetComponent<LookComponent>();
            
            _moveRequestComponent.SetAction(this);
            _moveRequestComponent.SetCondition(this);

            _healthComponent.OnDied += OnDied;

            _characterTrigger = GetComponentInChildren<TriggerComponent>();

            _characterTrigger.OnEntered += OnCharacterEnter;
            _characterTrigger.OnExited += OnCharacterExit;
            
            _collisionComponent = GetComponent<CollisionComponent>();
            
            _collisionComponent.OnEntered += OnCollisionEntered;
            
            _damageRequest = _damage.GetComponent<DamageRequestComponent>();
            _damageComponent = _damage.GetComponent<DamageComponent>();
            _cooldownComponent = _damage.GetComponent<CooldownComponent>();
            
            _damageRequest.SetCondition(this);
            _damageRequest.SetAction(this);
        }
        
        private void OnDestroy()
        {
            _healthComponent.OnDied -= OnDied;
            _characterTrigger.OnEntered -= OnCharacterEnter;
            _characterTrigger.OnExited -= OnCharacterExit;
            _collisionComponent.OnEntered -= OnCollisionEntered;
        }

        private void OnCollisionEntered(Collision2D col)
        {
            if(col.gameObject.layer != LayerMask.NameToLayer("Character"))
                return;
            _tossAbility.Use();
            _damageRequest.Damage(col.gameObject);
        }

        private void OnCharacterEnter(Collider2D col)
        {
            if(col.gameObject.layer == LayerMask.NameToLayer("Character"))
                _target = col.gameObject;
        }

        private void OnCharacterExit(Collider2D col)
        {
            if(col.gameObject.layer == LayerMask.NameToLayer("Character"))
                _target = null;
        }

        private void Update()
        {
            if(_target == null)
                return;

            Vector2 direction = new((_target.transform.position - transform.position).normalized.x, 0);
            _moveRequestComponent.Move(direction);
        }

        bool MoveRequestComponent.ICondition.Evaluate() =>
            _healthComponent.IsAlive;

        void MoveRequestComponent.IAction.Invoke(Vector2 direction)
        {
            _lookComponent.Look(direction.x);
            _moveComponent.Move(new Vector2(Mathf.Abs(direction.x), direction.y));
        }
        
        bool DamageRequestComponent.ICondition.Evaluate() => 
            _healthComponent.IsAlive &&
            _cooldownComponent.IsExpired;

        void DamageRequestComponent.IAction.Invoke(HealthComponent health)
        {
            _damageComponent.Apply(health);
            _cooldownComponent.Reset();
        }

        private void OnDied() => 
            GetComponent<Rigidbody2D>().simulated = false;
    }
}