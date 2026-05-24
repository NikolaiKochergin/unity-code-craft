using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public sealed class GameContextInstaller : SceneEntityInstaller<IGameContext>
    {
        [SerializeField] private GameEntity _playerCharacter;
        
        public override void Install(IGameContext context)
        {
            context.AddValue(GameContextAPI.Character, _playerCharacter);
            context.AddValue(GameContextAPI.Score, new ReactiveVariable<int>());
        }
    }
}