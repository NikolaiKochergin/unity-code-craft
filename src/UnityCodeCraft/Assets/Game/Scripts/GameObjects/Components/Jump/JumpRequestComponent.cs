using UnityEngine;

namespace Game
{
    public class JumpRequestComponent : CooldownComponent
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

        public void SetAction(IAction action) => _jumpAction = action;
        public void SetCondition(ICondition condition) => _jumpCondition = condition;
        
        public void Jump() => _jumpRequired = true;

        private void FixedUpdate()
        {
            if (_jumpRequired && IsExpired && (_jumpCondition == null || _jumpCondition.Evaluate()))
            {
                _jumpAction?.Invoke();
                Reset();
            } 

            _jumpRequired = false;
        }
    }
}