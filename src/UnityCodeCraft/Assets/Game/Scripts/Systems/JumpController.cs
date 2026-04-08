using UnityEngine;

namespace Game.Scripts.Systems
{
    public class JumpController : MonoBehaviour
    {
        [SerializeField] private GameObject _character;
        
        private JumpRequestComponent _jump;

        private void Awake() => 
            _jump = _character.GetComponent<JumpRequestComponent>();

        private void Update() => 
            HandleKeyboard();

        private void HandleKeyboard()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                _jump?.Jump();
        }
    }
}