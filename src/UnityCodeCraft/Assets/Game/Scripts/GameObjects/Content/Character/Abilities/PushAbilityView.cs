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
        
        private PushAbility _pushAbility;

        private void Awake()
        {
            _pushAbility = GetComponent<PushAbility>();
            _pushAbility.OnPush += OnPush;
        }

        private void OnDestroy() => 
            _pushAbility.OnPush -= OnPush;

        private void OnPush()
        {
            _animator.SetTrigger(BlowForward);
            _pushVFX.Play();
            _audioSource.PlayOneShot(_pushAudioClip);
        }
    }
}