using System;
using Modules.AI;
using UnityEngine;

namespace Game
{
    [Serializable]
    public sealed class ColliderBufferBlackboardInstaller : IBlackboardInstaller
    {
        [SerializeField] 
        private int _bufferSize = 8;
        
        public void Install(Blackboard blackboard)
        {
            blackboard.AddReferenceValue(BlackboardAPI.ColliderBuffer, new Collider[_bufferSize]);
            blackboard.AddPrimitiveValue(BlackboardAPI.ColliderBufferSize, 0);
        }
    }
}