using Modules.Utils;
using UnityEngine;

namespace Game
{
    public sealed class PlayerInputController : MonoBehaviour
    {
        [SerializeField] private Ship _ship;
        [SerializeField] private TransformBounds _playerArea;

        public void Update()
        {
            if(!_ship.Health.IsAlive)
                return;
            
            if (Input.GetKeyDown(KeyCode.Space))
                _ship.Attack.Fire();

            float dx = Input.GetAxisRaw("Horizontal");
            float dy = Input.GetAxisRaw("Vertical");
            
            _ship.Mover.SetDirection(new Vector2(dx, dy));
        }

        private void LateUpdate() => 
            _ship.transform.position = _playerArea.ClampInBounds(transform.position);
    }
}