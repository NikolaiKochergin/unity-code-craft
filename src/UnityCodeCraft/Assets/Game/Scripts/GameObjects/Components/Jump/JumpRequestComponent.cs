using System;
using UnityEngine;

namespace Game
{
    public class JumpRequestComponent : MonoBehaviour
    {
        public interface IAction
        {
            void Invoke();
        }

        public interface ICondition
        {
            bool Evaluate();
        }

        [SerializeField] private bool _jumpRequired;

        private IAction _jumpAction;
        private ICondition _jumpCondition;

        public event Action OnJumped;
        
        public void SetAction(IAction action) => _jumpAction = action;
        public void SetCondition(ICondition condition) => _jumpCondition = condition;
        
        public void Jump()
        {
            _jumpRequired = true;
        }

        private void FixedUpdate()
        {
            if (_jumpRequired && (_jumpCondition == null || _jumpCondition.Evaluate()))
            {
                _jumpAction?.Invoke();
                OnJumped?.Invoke();
            }

            _jumpRequired = false;
        }
    }
}