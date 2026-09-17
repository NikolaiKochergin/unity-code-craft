using Fusion;
using UnityEngine;

namespace Game
{
    public sealed class InputPolling : MonoBehaviour
    {
        [SerializeField] private NetworkEvents _networkEvents;
        [SerializeField] private InputMap _inputMap;

        private InputData _currentInput;
        private bool _resetInputs;

        private void Update()
        {
            if (_resetInputs)
            {
                _currentInput.buttons.Set(InputButtons.Mine, false);
                _currentInput.buttons.Set(InputButtons.Turret, false);
                _resetInputs = false;
            }

            _currentInput.moveDirection = _inputMap.GetMoveDirection();
            
            if(_inputMap.IsMinePressed()) 
                _currentInput.buttons.Set(InputButtons.Mine, true);
            
            if(_inputMap.IsTurretPressed())
                _currentInput.buttons.Set(InputButtons.Turret, true);
        }

        private void OnEnable() => 
            _networkEvents.OnInput.AddListener(OnInput);
        
        private void OnDisable() =>
            _networkEvents.OnInput.RemoveListener(OnInput);

        private void OnInput(NetworkRunner runner, NetworkInput input)
        {
            input.Set(_currentInput);
            _resetInputs = true;
        }
    }
}