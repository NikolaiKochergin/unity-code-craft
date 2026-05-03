using System;
using Atomic.Entities;
using Game.Gameplay;
using Game.UI;
using UnityEngine;

namespace Game.App
{
    [Serializable]
    public sealed class GameLoadInstaller : IEntityInstaller<IAppContext>
    {
        [SerializeField] private GameContext _gameContext;
        [SerializeField] private GameUI _gameUI;
        
        public void Install(IAppContext context)
        {
            context.AddValue(AppContextAPI.GameContext, _gameContext);
            context.AddValue(AppContextAPI.GameUI, _gameUI);
        }
    }
}