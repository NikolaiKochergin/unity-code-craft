using UnityEngine;

namespace Game.Scripts.Systems
{
    public class MoveController : MonoBehaviour
    {
        [SerializeField] private GameObject _character;
        
        private Character _moveComponent;

        private void Awake() => 
            _moveComponent = _character.GetComponent<Character>();

        private void Update() => 
            HandleKeyboard();

        private void HandleKeyboard()
        {
            Vector2 direction = Vector2.zero;
            
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                direction.x = 1;
            else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                direction.x = -1;

            if (direction != Vector2.zero)
                _moveComponent.Move(direction);
        }
    }
}