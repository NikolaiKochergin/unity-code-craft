using UnityEngine;
using Zenject;

namespace Game
{
    public class GameStartUp : MonoBehaviour
    {
        private GameCycle _gameCycle;
        
        [Inject]
        public void Construct(GameCycle gameCycle) => 
            _gameCycle = gameCycle;

        private void Start() => 
            _gameCycle.StartGame();
    }
}