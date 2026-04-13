using Game.Scripts.GameObjects.Components.Death;
using UnityEngine;

namespace Game.Scripts.GameObjects.Content.Spider
{
    public class Spider : MonoBehaviour,
        MoveRequestComponent.ICondition,
        MoveRequestComponent.IAction,
        DeathHandleComponent.IAction
    {
        private LookComponent _lookComponent;
        private HealthComponent _healthComponent;
        private DeathHandleComponent _deathHandleComponent;
        private WayPointRequestComponent _wayPointRequestComponent;
        private MoveTransformComponent _moveComponent;

        private void Awake()
        {
            _healthComponent = GetComponentInChildren<HealthComponent>();
            _deathHandleComponent = GetComponentInChildren<DeathHandleComponent>();
            _deathHandleComponent.SetAction(this);

            _moveComponent = GetComponentInChildren<MoveTransformComponent>();
            _wayPointRequestComponent = GetComponentInChildren<WayPointRequestComponent>();
            _lookComponent = GetComponentInChildren<LookComponent>();
            
            _wayPointRequestComponent.SetCondition(this);
            _wayPointRequestComponent.SetAction(this);
        }

        bool MoveRequestComponent.ICondition.Evaluate() =>
            _healthComponent.IsAlive;
        
        void MoveRequestComponent.IAction.Invoke(Vector2 direction)
        {
            _lookComponent.Look(direction.x);
            _moveComponent.Move(new Vector2(Mathf.Abs(direction.x), direction.y));
        }
        
        void DeathHandleComponent.IAction.Invoke() =>
            GetComponentInChildren<Rigidbody2D>().simulated = false;
    }
}