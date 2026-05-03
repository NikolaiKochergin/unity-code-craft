using UnityEngine;

namespace Game.Scripts.GameObjects.Content.Platform
{
    [RequireComponent(typeof(WaypointMoveComponent))]
    [RequireComponent(typeof(MoveTransformComponent))]
    public sealed class Platform : MonoBehaviour
    {
        private WaypointMoveComponent _waypointMoveComponent;
        private MoveTransformComponent _moveTransformComponent;

        private void Awake()
        {
            _waypointMoveComponent = GetComponent<WaypointMoveComponent>();
            _moveTransformComponent = GetComponent<MoveTransformComponent>();
            
            _waypointMoveComponent.SetAction(_moveTransformComponent.Move);
        }
    }
}