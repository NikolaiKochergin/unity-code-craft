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
                
            GameObject basePoint = new("BasePoint");
            basePoint.transform.position = blackboard.GetValue(BlackboardAPI.Character).transform.position;
            
            blackboard.SetReferenceValue(BlackboardAPI.BasePoint, basePoint);
        }
    }
}