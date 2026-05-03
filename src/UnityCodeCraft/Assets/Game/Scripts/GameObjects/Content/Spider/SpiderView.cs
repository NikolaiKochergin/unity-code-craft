using UnityEngine;

namespace Game
{
    public sealed class SpiderView : MonoBehaviour
    {
        private static readonly int Death = Animator.StringToHash("Death");
        private static readonly int IsGrounded = Animator.StringToHash("IsGrounded");
        private static readonly int IsMoving = Animator.StringToHash("IsMoving");

        [SerializeField] private GameObject _spider;
        [SerializeField] private Animator _animator;
        [SerializeField] private TakeDamageColorComponent _damageComponent;
        
        private HealthComponent _healthComponent;
        private GroundedComponent _groundedComponent;
        private WaypointMoveComponent _waypointMoveComponent;

        private void Awake()
        {
            _healthComponent = _spider.GetComponent<HealthComponent>();
            _groundedComponent = _spider.GetComponent<GroundedComponent>();
            _waypointMoveComponent = _spider.GetComponent<WaypointMoveComponent>();

            _healthComponent.OnHealthChanged += OnHealthChanged;
            _healthComponent.OnDied += OnDied;
            _groundedComponent.OnGrounded += OnGrounded;
        }

        private void OnDestroy()
        {
            _healthComponent.OnHealthChanged -= OnHealthChanged;
            _healthComponent.OnDied -= OnDied;
            _groundedComponent.OnGrounded -= OnGrounded;
        }

        private void Update() => 
            OnMove(_waypointMoveComponent.IsMoving);

        private void OnGrounded(bool isGrounded) => 
            _animator.SetBool(IsGrounded, _groundedComponent.IsGrounded);

        private void OnMove(bool isMoving) => 
            _animator.SetBool(IsMoving, isMoving);

        private void OnHealthChanged(float health) => 
            _damageComponent.TakeDamage();

        private void OnDied() => 
            _animator.SetTrigger(Death);
    }
}