using Atomic.Entities;
using Game.UI;
using UnityEngine;

namespace Game.Gameplay
{
    public sealed class PlayerContextInstaller : SceneEntityInstaller<IPlayerContext>
    {
        [SerializeField] private GameEntity _playerCharacter;
        
        public override void Install(IPlayerContext context)
        {
            context.AddValue(PlayerContextAPI.Character, _playerCharacter);
                
            context.AddBehaviour(new CharacterInputController());
        }
    }
}
