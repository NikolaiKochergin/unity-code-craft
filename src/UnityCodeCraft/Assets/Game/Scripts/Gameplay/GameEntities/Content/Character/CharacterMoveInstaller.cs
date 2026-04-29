using System;
using UnityEngine;

namespace Game.Gameplay
{
    [Serializable]
    public class CharacterMoveInstaller : IGameEntityInstaller
    {
        [SerializeField] private MoveInstaller _moveInstaller;
        
        public void Install(IGameEntity entity)
        {
            _moveInstaller.Install(entity);
        }
    }
}