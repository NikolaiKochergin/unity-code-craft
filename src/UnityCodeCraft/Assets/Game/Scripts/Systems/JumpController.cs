using UnityEngine;

namespace Game.Scripts.Systems
{
    public class JumpController : MonoBehaviour
    {
        [SerializeField] private GameObject _character;
        
        private Character _jumpComponent;

        private void Awake() => 
            _jumpComponent = _character.GetComponent<Character>();

        private void Update() => 
            HandleKeyboard();

        private void HandleKeyboard()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                _jumpComponent.Jump();
        }
    }
}