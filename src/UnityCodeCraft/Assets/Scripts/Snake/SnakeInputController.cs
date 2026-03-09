using Modules;
using UnityEngine;
using Zenject;

namespace Game
{
    public class SnakeInputController : ITickable
    {
        private readonly SnakeInputConfig _inputConfig;
        private readonly ISnake _snake;

        public SnakeInputController(ISnake snake, SnakeInputConfig inputConfig)
        {
            _snake = snake;
            _inputConfig = inputConfig;
        }

        public void Tick()
        {
            if(Input.GetKeyDown(_inputConfig.Left))
                _snake.Turn(SnakeDirection.LEFT);
            
            if(Input.GetKeyDown(_inputConfig.Right))
                _snake.Turn(SnakeDirection.RIGHT);
            
            if(Input.GetKeyDown(_inputConfig.Up))
                _snake.Turn(SnakeDirection.UP);
            
            if(Input.GetKeyDown(_inputConfig.Down))
                _snake.Turn(SnakeDirection.DOWN);
        }
    }
}
