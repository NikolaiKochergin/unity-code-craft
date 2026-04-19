using UnityEngine;

namespace Game.Scripts.GameObjects.Content.Monkey
{
    public class Monkey : MonoBehaviour
    {
        [SerializeField] private HealthComponent _health;
        [SerializeField] private JumpAbility _jumpAbility;
        [SerializeField] private LookComponent _look;
        [SerializeField] private TriggerComponent _characterTrigger;
        [SerializeField] private GroundedComponent _grounded;
        [SerializeField] private CollisionComponent _collision;
        [SerializeField] private DamageComponent _damage;
        // [SerializeField] private PushAbility _pushAbility;
        
        private GameObject _target;

        private void Awake()
        {
            _characterTrigger.OnEntered += OnCharacterEntered;
            _characterTrigger.OnExited += OnCharacterExited;
            _collision.OnEntered += OnCollisionEntered;
            _health.OnDied += OnDead;
            _grounded.OnGrounded += OnGrounded;
        }

        private void OnDestroy()
        {
            _characterTrigger.OnEntered -= OnCharacterEntered;
            _characterTrigger.OnExited -= OnCharacterExited;
            _collision.OnEntered -= OnCollisionEntered;
            _health.OnDied -= OnDead;
            _grounded.OnGrounded -= OnGrounded;
        }

        private void OnCharacterEntered(Collider2D col)
        {
            if(col.gameObject.layer == LayerMask.NameToLayer("Character"))
                _target = col.gameObject;
        }

        private void OnCharacterExited(Collider2D col)
        {
            if(col.gameObject.layer == LayerMask.NameToLayer("Character"))
                _target = null;
        }

        private void Update()
        {
            if(_health.IsDied)
                return;
            
            if(_target)
                _look.Look(_target.transform);
            
            _jumpAbility.Jump();
        }

        private void OnCollisionEntered(Collision2D col)
        {
            if(col.gameObject.TryGetComponent(out HealthComponent health))
                _damage.Apply(health);
        }

        private void OnDead() => 
            GetComponent<Rigidbody2D>().simulated = false;

        private void OnGrounded(bool isGrounded)
        {
            // if(isGrounded)
            //     _pushAbility.Use();
        }
    }
}