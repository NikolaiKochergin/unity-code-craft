using UnityEngine;

namespace Game.Scripts.GameObjects.Components.Death
{
    [RequireComponent(typeof(HealthComponent))]
    public class DeathHandleComponent : MonoBehaviour
    {
        public interface IAction
        {
            void Invoke();
        }
        
        private HealthComponent _healthComponent;
        
        private IAction _deathAction;

        private void Awake()
        {
            _healthComponent = GetComponent<HealthComponent>();
            _healthComponent.OnDied += OnDie;
        }

        private void OnDestroy() => 
            _healthComponent.OnDied -= OnDie;
        
        public void SetAction(IAction action) => _deathAction = action;

        private void OnDie() => _deathAction?.Invoke();
    }
}
