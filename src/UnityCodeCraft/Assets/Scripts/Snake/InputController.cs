using Modules;
using UnityEngine;
using Zenject;

namespace Gameplay.GameContext
{
    public class InputController : ITickable
    {
        private readonly ISnake _snake;

        public InputController(ISnake snake) => 
            _snake = snake;

        public void Tick()
        {
            if(Input.GetKeyDown(KeyCode.A))
                _snake.Turn(SnakeDirection.LEFT);
            
            if(Input.GetKeyDown(KeyCode.D))
                _snake.Turn(SnakeDirection.RIGHT);
            
            if(Input.GetKeyDown(KeyCode.W))
                _snake.Turn(SnakeDirection.UP);
            
            if(Input.GetKeyDown(KeyCode.S))
                _snake.Turn(SnakeDirection.DOWN);
        }
    }
}
