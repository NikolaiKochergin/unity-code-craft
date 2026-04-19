using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    public sealed class FallingComponent : MonoBehaviour
    {
        [SerializeField] private ExtraGravityComponent _extraGravity;
        
        [ShowInInspector, ReadOnly, HideInEditorMode]
        private bool _isFalling;

        private Rigidbody2D _rigidbody;
        
        public bool IsFalling => _isFalling;

        public event Action<bool> OnFalling;

        private void Awake() => 
            _rigidbody = GetComponent<Rigidbody2D>();

        private void FixedUpdate()
        {
            bool isFalling = _rigidbody.linearVelocityY < 0;

            if (isFalling == _isFalling) 
                return;
            _isFalling = isFalling;
            
            if(_extraGravity)
                _extraGravity.enabled = _isFalling;
            
            OnFalling?.Invoke(_isFalling);
        }
    }
}