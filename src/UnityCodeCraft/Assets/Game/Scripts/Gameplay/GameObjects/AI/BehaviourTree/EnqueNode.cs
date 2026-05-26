using Modules.AI;
using UnityEngine;

namespace Game
{
    public class EnqueNode : BehaviourNode
    {
        [SerializeField]
        private Blackboard _blackboard;
        
        protected override BehaviourResult OnUpdate(float deltaTime)
        {
            if(_blackboard.GetValue(BlackboardAPI.CommandQueue).TryDequeue(out IUnitCommand result))
                result.Begin();
            
            return BehaviourResult.Success;
        }
    }
}