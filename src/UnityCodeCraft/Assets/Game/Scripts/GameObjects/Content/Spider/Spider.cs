using UnityEngine;

namespace Game.Scripts.GameObjects.Content.Spider
{
    public class Spider : MonoBehaviour,
        DamageRequestComponent.ICondition,
        DamageRequestComponent.IAction
    {
        [SerializeField] private GameObject _pushAbility;
        
        private LookComponent _lookComponent;
        private HealthComponent _healthComponent;
        private WayPointComponent _wayPointRequestComponent;
        private MoveTransformComponent _moveComponent;
        private CollisionComponent _collisionComponent;
        private DamageComponent _damageComponent;
        private DamageRequestComponent _damageRequest;
        private CooldownComponent _cooldownComponent;
        // private AttackRequestComponent<Rigidbody2D> _pushRequest;
        private ForceComponent _pushComponent;
        private CooldownComponent _pushCooldown;

        private void Awake()
        {
            _healthComponent = GetComponentInChildren<HealthComponent>();

            _moveComponent = GetComponentInChildren<MoveTransformComponent>();
            _wayPointRequestComponent = GetComponentInChildren<WayPointComponent>();
            _lookComponent = GetComponentInChildren<LookComponent>();
            
            // _wayPointRequestComponent.SetCondition(this);
            // _wayPointRequestComponent.SetAction(this);
            
            _collisionComponent = GetComponentInChildren<CollisionComponent>();

            _damageRequest = GetComponentInChildren<DamageRequestComponent>();
            _damageComponent = GetComponentInChildren<DamageComponent>();
            _cooldownComponent = GetComponentInChildren<CooldownComponent>();
            
            _damageRequest.SetCondition(this);
            _damageRequest.SetAction(this);

            // _pushRequest = _pushAbility.GetComponent<PushRequestComponent>();
            // _pushComponent = _pushAbility.gameObject.AddComponent<ForceComponent>();
            // _pushCooldown = _pushAbility.GetComponent<CooldownComponent>();
            //
            // _pushRequest.SetCondition(this);
            // _pushRequest.SetAction(this);

            _healthComponent.OnDied += OnDied;
            _collisionComponent.OnEntered += OnCollisionEntered;
        }

        private void OnDestroy()
        {
            _healthComponent.OnDied -= OnDied;
            _collisionComponent.OnEntered -= OnCollisionEntered;
        }

        private void OnCollisionEntered(Collision2D target)
        {
            if(target.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                return;
            
            _damageRequest.Damage(target.gameObject);
            // _pushRequest.Require(target.gameObject);
        }

        private void OnDied() =>
            GetComponentInChildren<Rigidbody2D>().simulated = false;

        // bool MoveRequestComponent.ICondition.Evaluate() =>
        //     _healthComponent.IsAlive;
        //
        // void MoveRequestComponent.IAction.Invoke(Vector2 direction)
        // {
        //     _lookComponent.Look(direction.x);
        //     _moveComponent.Move(new Vector2(Mathf.Abs(direction.x), direction.y));
        // }

        bool DamageRequestComponent.ICondition.Evaluate() => 
            _healthComponent.IsAlive &&
            _cooldownComponent.IsExpired;

        void DamageRequestComponent.IAction.Invoke(HealthComponent health)
        {
            _damageComponent.Apply(health);
            _cooldownComponent.Reset();
        }

        // bool AttackRequestComponent<Rigidbody2D>.ICondition.Evaluate() =>
        //     _healthComponent.IsAlive &&
        //     _pushCooldown.IsExpired;
        //
        // void AttackRequestComponent<Rigidbody2D>.IAction.Invoke(Rigidbody2D rb) => 
        //     _pushComponent.ApplyTo(rb.transform, 
        //         new Vector2((rb.transform.position - transform.position).normalized.x, 0));
    }
}