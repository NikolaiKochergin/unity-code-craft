using System;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(ExtraGravityComponent))]
    public class FallingHandleComponent : MonoBehaviour
    {
        [SerializeField, Min(0)] private float _velocityCheckDelta = 0.01f;
        
        private Rigidbody2D _rigidbody;
        private ExtraGravityComponent _extraGravityComponent;
        
        private bool _isFalling;

        public bool IsFalling
        {
            get => _isFalling;
            set
            {
                if(value == _isFalling)
                    return;
                
                _isFalling = value;
                OnFalling?.Invoke(value);
            }
        }
        
        public event Action<bool> OnFalling;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _extraGravityComponent = GetComponent<ExtraGravityComponent>();
        }

        private void FixedUpdate()
        {
            IsFalling = _rigidbody && _rigidbody.linearVelocityY < -_velocityCheckDelta;
            _extraGravityComponent.enabled = IsFalling;
        }
    }
}
