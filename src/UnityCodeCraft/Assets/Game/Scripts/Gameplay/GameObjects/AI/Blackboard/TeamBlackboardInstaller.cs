using System;
using Modules.AI;
using SampleGame;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class TeamBlackboardInstaller : IBlackboardInstaller
    {
        [SerializeField] private GameObject _character;
        
        public void Install(Blackboard blackboard)
        {
            if(_character.TryGetComponent(out TeamComponent teamComponent))
                blackboard.SetPrimitiveValue(BlackboardAPI.Team, teamComponent.Team);
        }
    }
}