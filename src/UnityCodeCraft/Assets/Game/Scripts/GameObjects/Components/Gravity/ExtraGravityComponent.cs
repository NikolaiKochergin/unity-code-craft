using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(GroundedComponent), typeof(Rigidbody2D))]
    public sealed class ExtraGravityComponent : MonoBehaviour
    {
        [SerializeField]
        private float _gravity = -7f;
        
        private GroundedComponent _groundedComponent;
        private Rigidbody2D _rigidbody;

        private void Awake()
        {
            _groundedComponent = GetComponent<GroundedComponent>();
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            if (!_groundedComponent.IsGrounded && _rigidbody.linearVelocityY < 0)
                _rigidbody.linearVelocity += new Vector2(0, _gravity * Time.fixedDeltaTime);
        }
    }
}