using UnityEngine;

namespace Game
{
    public class CooldownComponent : MonoBehaviour
    {
        [SerializeField] private float _cooldown = 0.35f;

        private float _currentTime;
        
        public bool IsExpired => Time.time - _currentTime >= _cooldown;

        private void Awake() => 
            _currentTime = Time.time - _cooldown;

        public void Reset() => 
            _currentTime = Time.time;
    }
}