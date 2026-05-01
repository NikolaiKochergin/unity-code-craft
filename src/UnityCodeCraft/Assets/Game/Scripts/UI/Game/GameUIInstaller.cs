using Atomic.Entities;
using Game.Modules;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public sealed class GameUIInstaller : SceneEntityInstaller<IGameUI>
    {
        [SerializeField] private HealthScreenView _healthScreenView;
        [SerializeField] private Joystick _moveJoystick;
        [SerializeField] private Joystick _attackJoystick;
        [SerializeField] private StatView _healthView;
        [SerializeField] private StatView _ammoView;
        [SerializeField] private TMP_Text _killsView;
        
        public override void Install(IGameUI ui)
        {
            PlayerContext playerContext = PlayerContext.Instance;
            
            ui.AddValue(GameUIAPI.MoveJoystick, _moveJoystick);
            ui.AddValue(GameUIAPI.AttackJoystick, _attackJoystick);
            
            ui.AddBehaviour(new UIInputPresenter(ui, playerContext));
        }
    }
}
