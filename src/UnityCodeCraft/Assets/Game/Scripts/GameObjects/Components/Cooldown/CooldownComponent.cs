using UnityEngine;

namespace Game
{
    public class CooldownComponent : MonoBehaviour
    {
        [SerializeField] private float _cooldown;

        private float _currentTime;
        
        public bool IsExpired => Time.time - _currentTime >= _cooldown;

        private void Awake() => 
            _currentTime = Time.time - _cooldown;

        public void Reset() => 
            _currentTime = Time.time;
    }
}