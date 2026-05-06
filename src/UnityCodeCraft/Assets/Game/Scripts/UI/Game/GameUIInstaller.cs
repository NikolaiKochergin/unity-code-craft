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
        
        public override void Install(IGameUI ui)
        {
            IPlayerContext playerContext = GameContext.Instance.GetValue(GameContextAPI.PlayerContext);
            
            ui.AddValue(GameUIAPI.MoveJoystick, _moveJoystick);
            ui.AddValue(GameUIAPI.AimJoystick, _aimJoystick);
            ui.AddValue(GameUIAPI.HealthScreenView, _healthScreenView);
            ui.AddValue(GameUIAPI.HealthView, _healthView);
            ui.AddValue(GameUIAPI.AmmoView, _ammoView);
            ui.AddValue(GameUIAPI.KillsView, _killsView);
            
            ui.AddBehaviour(new HealthViewPresenter(playerContext));
            ui.AddBehaviour(new AmmoViewPresenter(playerContext));
        }
    }
}
