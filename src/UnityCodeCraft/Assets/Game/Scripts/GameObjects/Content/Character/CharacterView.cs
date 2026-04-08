using UnityEngine;

namespace Game
{
    public class CharacterView : MonoBehaviour
    {
        private static readonly int IsMoving = Animator.StringToHash("IsMoving");
        private static readonly int IsGrounded = Animator.StringToHash("IsGrounded");
        private static readonly int Jump = Animator.StringToHash("Jump");
        
        [SerializeField] private Animator _animator;
        
        private LookComponent _lookComponent;
        private MoveRequestComponent _moveComponent;
        private GroundedComponent _groundedComponent;
        private JumpRequestComponent _jumpComponent;

        private void Awake()
        {
            _lookComponent = GetComponent<LookComponent>();
            _moveComponent = GetComponentInParent<MoveRequestComponent>();
            _jumpComponent = GetComponentInParent<JumpRequestComponent>();
            _groundedComponent = GetComponentInParent<GroundedComponent>();
            
            _moveComponent.OnMoved += OnMoved;
            _jumpComponent.OnJumped += OnJumped;
        }

        private void OnDestroy()
        {
            _moveComponent.OnMoved -= OnMoved;
            _jumpComponent.OnJumped -= OnJumped;
        }

        private void Update()
        {
            _animator.SetBool(IsMoving, _moveComponent.IsMoving);
            _animator.SetBool(IsGrounded, _groundedComponent.IsGrounded);
        }

        private void OnJumped() => 
            _animator.SetTrigger(Jump);

        private void OnMoved(Vector2 direction) => 
            _lookComponent.Look(direction.x);
    }
}