using UnityEngine;

namespace Game
{
    public class JumpComponent : ActionComponent
    {
        [SerializeField, Min(0)] private float _jumpForce;
        
        private Rigidbody2D _rigidbody2D;

        private void Awake() => 
            _rigidbody2D = GetComponentInParent<Rigidbody2D>();

        public override void Apply() => 
            _rigidbody2D.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
    }
}