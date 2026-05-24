using Atomic.Entities;
using Game.Gameplay;
using Game.Modules;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public sealed class GameUIInstaller : SceneEntityInstaller<IGameUI>
    {
        [SerializeField] private Joystick _moveJoystick;
        [SerializeField] private Joystick _aimJoystick;
        [SerializeField] private HealthScreenView _healthScreenView;
        [SerializeField] private StatView _healthView;
        [SerializeField] private StatView _ammoView;
        [SerializeField] private TMP_Text _killsView;
        [SerializeField] private PlayerInputInstaller _playerInputInstaller;
        
        
        public override void Install(IGameUI ui)
        {
            ui.AddValue(GameUIAPI.GameContext, GameContext.Instance);
            ui.AddValue(GameUIAPI.MoveJoystick, _moveJoystick);
            ui.AddValue(GameUIAPI.AimJoystick, _aimJoystick);
            ui.AddValue(GameUIAPI.HealthScreenView, _healthScreenView);
            ui.AddValue(GameUIAPI.HealthView, _healthView);
            ui.AddValue(GameUIAPI.AmmoView, _ammoView);
            ui.AddValue(GameUIAPI.KillsView, _killsView);
            
            ui.AddBehaviour<HealthViewPresenter>();
            ui.AddBehaviour<AmmoViewPresenter>();
            ui.AddBehaviour<KillsViewPresenter>();
            ui.AddBehaviour<HealthScreenViewPresenter>();
            
            _playerInputInstaller.Install(ui);
        }
    }
}
