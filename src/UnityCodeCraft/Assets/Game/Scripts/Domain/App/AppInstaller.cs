using Modules.Extensions;
using UnityEngine;
using Zenject;

namespace Game.Scripts.Domain.App
{
    [CreateAssetMenu(
        fileName = "AppInstaller",
        menuName = "Zenject/New AppInstaller"
    )]
    public class AppInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private RepositoryInstaller _repositoryInstaller;

        public override void InstallBindings()
        {
            Container
                .Install(_repositoryInstaller);
        }
    }
}