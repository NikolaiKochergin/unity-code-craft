using System;
using UnityEngine;

namespace Game
{
    public class PushRequestComponent : CooldownComponent
    {
        public interface IAction
        {
            void Invoke();
        }

        public interface ICondition
        {
            bool Evaluate();
        }

        [SerializeField] private bool _pushRequired;

        private IAction _pushAction;
        private ICondition _pushCondition;

        public event Action OnPushed;
        
        public void Push()
        {
            
        }
    }
}