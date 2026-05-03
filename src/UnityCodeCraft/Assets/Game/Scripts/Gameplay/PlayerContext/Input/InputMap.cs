using UnityEngine;

namespace Game.Gameplay
{
    [CreateAssetMenu(
        menuName = "Game/New InputMap",
        fileName = "InputMap"
    )]
    public class InputMap : ScriptableObject
    {
        [Header("Move Input")] 
        [SerializeField] private KeyCode _moveForward = KeyCode.W;
        [SerializeField] private KeyCode _moveBack = KeyCode.S;
        [SerializeField] private KeyCode _moveLeft = KeyCode.A;
        [SerializeField] private KeyCode _moveRight = KeyCode.D;

        [Space] [Header("Aim Input")] 
        [SerializeField] private KeyCode _aimForward = KeyCode.UpArrow;
        [SerializeField] private KeyCode _aimBack = KeyCode.DownArrow;
        [SerializeField] private KeyCode _aimLeft = KeyCode.LeftArrow;
        [SerializeField] private KeyCode _aimRight = KeyCode.RightArrow;

        public Vector2 GetMoveDirection()
        {
            Vector2 direction = Vector2.zero;

            if (Input.GetKey(_moveForward))
                direction.y = 1;
            else if (Input.GetKey(_moveBack))
                direction.y = -1;

            if (Input.GetKey(_moveLeft))
                direction.x = -1;
            else if (Input.GetKey(_moveRight))
                direction.x = 1;

            return direction;
        }
        
        public Vector2 GetAimDirection()
        {
            Vector2 direction = Vector2.zero;

            if (Input.GetKey(_aimForward))
                direction.y = 1;
            else if (Input.GetKey(_aimBack))
                direction.y = -1;

            if (Input.GetKey(_aimLeft))
                direction.x = -1;
            else if (Input.GetKey(_aimRight))
                direction.x = 1;

            return direction;
        }
    }
}