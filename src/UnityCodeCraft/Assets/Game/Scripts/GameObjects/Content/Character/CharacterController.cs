using UnityEngine;

namespace Game
{
    public class CharacterController : MonoBehaviour
    {
        [SerializeField] private GameObject _character;
        
        private IMoveComponent _moveComponent;
        private IJumpComponent _jumpComponent;
        private IFireComponent _fireComponent;

        private void Awake()
        {
            _moveComponent = _character.GetComponent<IMoveComponent>();
            _jumpComponent = _character.GetComponent<IJumpComponent>();
            _fireComponent = _character.GetComponent<IFireComponent>();
        }

        private void Update()
        {
            HandleKeyboard();
            HandleMouse();
        }

        private void HandleKeyboard()
        {
            Vector2 direction = Vector2.zero;
            
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                direction.x = 1;
            else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                direction.x = -1;

            if (direction != Vector2.zero)
                _moveComponent.Move(direction);
            
            if (Input.GetKeyDown(KeyCode.Space))
                _jumpComponent.Jump();
        }

        private void HandleMouse()
        {
            if (Input.GetMouseButtonDown(0))
                _fireComponent.Push();
            
            if (Input.GetMouseButtonDown(1))
                _fireComponent.Toss();
        }
    }
}