using Modules;
using UnityEngine;
using Zenject;

namespace Game
{
    public class InputController : ITickable
    {
        private readonly ISnake _snake;

        public InputController(ISnake snake) => 
            _snake = snake;

        public void Tick()
        {
            if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
                _snake.Turn(SnakeDirection.LEFT);
            
            if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                _snake.Turn(SnakeDirection.RIGHT);
            
            if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                _snake.Turn(SnakeDirection.UP);
            
            if(Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
                _snake.Turn(SnakeDirection.DOWN);
        }
    }
}
