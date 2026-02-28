using UnityEngine;

namespace Game
{
    public sealed class PlayerInputController : MonoBehaviour
    {
        [SerializeField] private Ship _ship;

        public void Update()
        {
            if(!_ship.IsAlive)
                return;
            
            if (Input.GetKeyDown(KeyCode.Space))
                _ship.Fire(_ship.FirePoint.up);

            float dx = Input.GetAxisRaw("Horizontal");
            float dy = Input.GetAxisRaw("Vertical");
            
            _ship.Direction = new Vector2(dx, dy);
        }
    }
}