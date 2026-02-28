using Modules.Utils;
using UnityEngine;

namespace Game
{
    public class PlayerShipClamping : MonoBehaviour
    {
        [SerializeField] private Ship _ship;
        [SerializeField] private TransformBounds _playerArea;
        
        private void LateUpdate() => 
            _ship.Position = _playerArea.ClampInBounds(_ship.Position);
    }
}