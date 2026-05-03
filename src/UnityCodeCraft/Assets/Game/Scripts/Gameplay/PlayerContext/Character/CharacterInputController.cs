using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public sealed class CharacterInputController : IPlayerContextInit, IPlayerContextTick
    {
        private IGameEntity _character;

        public void Init(IPlayerContext context)
        {
            _character = context.GetValue(PlayerContextAPI.Character);
        }

        public void Tick(IPlayerContext entity, float deltaTime)
        {
            ProcessMove();
        }

        private void ProcessMove()
        {
            Vector3 direction = GetMoveDirection();

            _character
                .GetValue(GameEntityAPI.MoveRequest)
                .Invoke(direction);
        }
        
        
        public KeyCode MoveForward = KeyCode.W;
        public KeyCode MoveBack = KeyCode.S;
        public KeyCode MoveLeft = KeyCode.A;
        public KeyCode MoveRight = KeyCode.D;
        
        public Vector3 GetMoveDirection()
        {
            Vector3 direction = Vector3.zero;

            if (Input.GetKey(MoveForward))
                direction.z = 1;
            else if (Input.GetKey(MoveBack))
                direction.z = -1;

            if (Input.GetKey(MoveLeft))
                direction.x = -1;
            else if (Input.GetKey(MoveRight))
                direction.x = 1;
            
            return direction;
        }
    }
}