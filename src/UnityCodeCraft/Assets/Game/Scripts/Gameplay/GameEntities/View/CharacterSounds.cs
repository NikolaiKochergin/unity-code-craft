using UnityEngine;

namespace Game.Gameplay
{
    public class CharacterSounds : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _death;
        [SerializeField] private AudioClip[] _moveSteps;

        public void PlayMoveStep() => 
            _audioSource.PlayOneShot(_moveSteps[Random.Range(0, _moveSteps.Length)]);
        
        public void PlayDeathSound() =>
            _audioSource.PlayOneShot(_death);
    }
}