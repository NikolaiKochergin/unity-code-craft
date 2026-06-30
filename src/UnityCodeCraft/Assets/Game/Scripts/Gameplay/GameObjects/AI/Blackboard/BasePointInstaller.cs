using System;
using Modules.AI;
using UnityEngine;

namespace Game
{
    [Serializable]
    public sealed class BasePointInstaller : IBlackboardInstaller
    {
        public void Install(Blackboard blackboard)
        {
            if(!Application.isPlaying)
                return;
            
            Vector3 characterPosition = blackboard.GetValue(BlackboardAPI.Character).transform.position;
            blackboard.SetReferenceValue(BlackboardAPI.BasePoint, new PositionWayPoint(characterPosition));
        }
    }
}