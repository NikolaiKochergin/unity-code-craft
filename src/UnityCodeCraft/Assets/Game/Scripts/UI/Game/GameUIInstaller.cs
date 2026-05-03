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
        [SerializeField] private Joystick _attackJoystick;
        [SerializeField] private HealthScreenView _healthScreenView;
        [SerializeField] private StatView _healthView;
        [SerializeField] private StatView _ammoView;
        [SerializeField] private TMP_Text _killsView;
        
        public override void Install(IGameUI ui)
        {
            PlayerContext playerContext = GameContext.Instance.GetValue(GameContextAPI.PlayerContext);
            
            ui.AddValue(GameUIAPI.MoveJoystick, _moveJoystick);
            ui.AddValue(GameUIAPI.AttackJoystick, _attackJoystick);
            ui.AddValue(GameUIAPI.HealthScreenView, _healthScreenView);
            ui.AddValue(GameUIAPI.HealthView, _healthView);
            ui.AddValue(GameUIAPI.AmmoView, _ammoView);
            ui.AddValue(GameUIAPI.KillsView, _killsView);
            
            ui.AddBehaviour(new UIInputPresenter(playerContext));
            ui.AddBehaviour(new StatPresenter(playerContext));
        }
    }
}
