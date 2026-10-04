using TMPro;
using UnityEngine;
using Zenject;

namespace Game
{
    public sealed class GameUIInstaller : MonoInstaller
    {
        [SerializeField] private SmoothHealthBar _portalHealthBar;
        [SerializeField] private TMP_Text _moneyAmountText;

        public override void InstallBindings()
        {
            Container.Bind<SmoothHealthBar>().WithId(Tags.Portal).FromInstance(_portalHealthBar).AsSingle();
            Container.Bind<TMP_Text>().WithId(Tags.Player).FromInstance(_moneyAmountText).AsSingle();
        }
    }
}