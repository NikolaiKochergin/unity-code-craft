using Atomic.Entities;
using UnityEngine;

namespace Game.App
{
    public sealed class AppContextInstaller : SceneEntityInstaller<IAppContext>
    {
        [SerializeField] private GameLoadInstaller _loadGameInstaller;
        
        public override void Install(IAppContext context)
        {
            _loadGameInstaller.Install(context);
        }
    }
}