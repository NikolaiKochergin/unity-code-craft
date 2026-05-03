using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public sealed class GameContextInstaller : SceneEntityInstaller<IGameContext>
    {
        [SerializeField] private PlayerContext _playerContext;
        
        public override void Install(IGameContext context)
        {
            context.AddValue(GameContextAPI.PlayerContext, _playerContext);
        }
    }
}