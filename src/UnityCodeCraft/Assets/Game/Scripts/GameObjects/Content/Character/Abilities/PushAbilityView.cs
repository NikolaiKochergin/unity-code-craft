using UnityEngine;

namespace Game
{
    public class PushAbilityView : MonoBehaviour
    {
        private static readonly int BlowForward = Animator.StringToHash("BlowForward");
        
        [SerializeField] private ForceAbility _pushAbility;
        [SerializeField] private Animator _animator;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _pushAudioClip;
        [SerializeField] private ParticleSystem _pushVFX;
        

        private void Awake() => 
            _pushAbility.OnApplied += OnPush;

        private void OnDestroy() => 
            _pushAbility.OnApplied -= OnPush;

        private void OnPush()
        {
            _animator.SetTrigger(BlowForward);
            _pushVFX.Play();
            _audioSource.PlayOneShot(_pushAudioClip);
        }
    }
}