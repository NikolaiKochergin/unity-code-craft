using UnityEngine;

namespace Game
{
    public class PushAbilityView : MonoBehaviour
    {
        private static readonly int BlowForward = Animator.StringToHash("BlowForward");
        
        [SerializeField] private Animator _animator;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _pushAudioClip;
        [SerializeField] private ParticleSystem _pushVFX;
        
        private PushComponent _pushComponent;

        private void Awake()
        {
            _pushComponent = GetComponent<PushComponent>();
            _pushComponent.OnPush += OnPush;
        }

        private void OnDestroy() => 
            _pushComponent.OnPush -= OnPush;

        private void OnPush()
        {
            _animator.SetTrigger(BlowForward);
            _pushVFX.Play();
            _audioSource.PlayOneShot(_pushAudioClip);
        }
    }
}