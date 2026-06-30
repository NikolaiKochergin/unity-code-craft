using Modules.AI;

namespace Game
{
    public class IdleNode : BehaviourNode
    {
        protected override BehaviourResult OnUpdate(float deltaTime) => 
            BehaviourResult.Running;
    }
}