using System;
using Atomic.Entities;
using UnityEngine;

namespace Game.UI
{
    [Serializable]
    public class PlayerInputInstaller : IGameUIInstaller
    {
        [SerializeField] private InputMap _inputMap;
        
        public void Install(IGameUI entity)
        {
            entity.AddValue(GameUIAPI.InputMap, _inputMap);
            
            entity.AddBehaviour<JoystickMoveController>();
            entity.AddBehaviour<JoystickAimController>();
            entity.AddBehaviour<KeyboardMoveController>();
            entity.AddBehaviour<KeyboardAimController>();
        }
    }
}