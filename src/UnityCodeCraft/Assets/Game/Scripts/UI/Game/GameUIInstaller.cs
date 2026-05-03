using Atomic.Elements;
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
            ui.AddValue(GameUIAPI.MoveJoystick, _moveJoystick);
            ui.AddValue(GameUIAPI.AttackJoystick, _attackJoystick);
            
            ui.AddBehaviour(new UIInputPresenter(ui, GameContext.Instance));
            
            // IReactiveVariable<int> currentHealth = playerContext
            //     .GetValue(PlayerContextAPI.Character)
            //     .GetValue(GameEntityAPI.CurrentHealth);
            //
            // Const<int> maxHealth = playerContext
            //     .GetValue(PlayerContextAPI.Character)
            //     .GetValue(GameEntityAPI.MaxHealth);
            //
            // ui.AddBehaviour(new StatPresenter(_healthView, currentHealth, maxHealth));
        }
    }
}
