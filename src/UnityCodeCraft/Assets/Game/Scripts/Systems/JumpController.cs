using UnityEngine;

namespace Game.Scripts.Systems
{
    public class JumpController : MonoBehaviour
    {
        [SerializeField] private GameObject _character;
        
        private IJumpComponent _jump;

        private void Awake() => 
            _jump = _character.GetComponent<IJumpComponent>();

        private void Update() => 
            HandleKeyboard();

        private void HandleKeyboard()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                _jump?.Jump();
        }
    }
}