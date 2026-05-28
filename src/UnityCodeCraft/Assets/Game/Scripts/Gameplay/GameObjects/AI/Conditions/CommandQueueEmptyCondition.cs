using System;
using Modules.AI;
using UnityEngine;

namespace Game
{
    [Serializable]
    public sealed class CommandQueueEmptyCondition : ICondition
    {
        [SerializeField] 
        private Blackboard _blackboard;
        
        public bool Invoke() => 
            _blackboard.GetValue(BlackboardAPI.CommandQueue).IsEmpty;
    }
}