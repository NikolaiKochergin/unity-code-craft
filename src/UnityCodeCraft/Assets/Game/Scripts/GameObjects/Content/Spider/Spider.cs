using System;
using UnityEngine;

namespace Game.Scripts.GameObjects.Content.Spider
{
    public class Spider : MonoBehaviour,
        MoveRequestComponent.ICondition,
        MoveRequestComponent.IAction
    {
        private LookComponent _lookComponent;
        private HealthComponent _healthComponent;
        private WayPointRequestComponent _wayPointRequestComponent;
        private MoveTransformComponent _moveComponent;

        private void Awake()
        {
            _healthComponent = GetComponentInChildren<HealthComponent>();

            _moveComponent = GetComponentInChildren<MoveTransformComponent>();
            _wayPointRequestComponent = GetComponentInChildren<WayPointRequestComponent>();
            _lookComponent = GetComponentInChildren<LookComponent>();
            
            _wayPointRequestComponent.SetCondition(this);
            _wayPointRequestComponent.SetAction(this);

            _healthComponent.OnDied += OnDied;
        }

        private void OnDestroy()
        {
            _healthComponent.OnDied -= OnDied;
        }

        bool MoveRequestComponent.ICondition.Evaluate() =>
            _healthComponent.IsAlive;
        
        void MoveRequestComponent.IAction.Invoke(Vector2 direction)
        {
            _lookComponent.Look(direction.x);
            _moveComponent.Move(new Vector2(Mathf.Abs(direction.x), direction.y));
        }
        
        private void OnDied() =>
            GetComponentInChildren<Rigidbody2D>().simulated = false;
    }
}