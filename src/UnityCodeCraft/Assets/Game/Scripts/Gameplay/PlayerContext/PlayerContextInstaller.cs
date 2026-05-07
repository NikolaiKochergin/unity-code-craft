using Atomic.Elements;
using Atomic.Entities;
using Game.UI;
using UnityEngine;

namespace Game.Gameplay
{
    public sealed class PlayerContextInstaller : SceneEntityInstaller<IPlayerContext>
    {
        [SerializeField] private GameEntity _playerCharacter;
        [SerializeField] private InputMap _inputMap;
        
        public override void Install(IPlayerContext context)
        {
            context.AddValue(PlayerContextAPI.Character, _playerCharacter);
            context.AddValue(PlayerContextAPI.InputMap, _inputMap);
            context.AddValue(PlayerContextAPI.Score, new ReactiveVariable<int>());
                
            context.AddBehaviour(new CharacterInputController(GameUI.Instance));
        }
    }
}
