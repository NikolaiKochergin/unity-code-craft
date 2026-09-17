using UnityEngine;

namespace Game
{
    [CreateAssetMenu(
        fileName = "InputMap",
        menuName = "Game/New InputMap"
    )]
    public sealed class InputMap : ScriptableObject
    {
        [SerializeField] private KeyCode _mineKey = KeyCode.Q;
        [SerializeField] private KeyCode _turretKey = KeyCode.E;

        public Vector2 GetMoveDirection() =>
            new(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        public bool IsMinePressed() => 
            Input.GetKeyDown(_mineKey);
        
        public bool IsTurretPressed() =>
            Input.GetKeyDown(_turretKey);
    }
}