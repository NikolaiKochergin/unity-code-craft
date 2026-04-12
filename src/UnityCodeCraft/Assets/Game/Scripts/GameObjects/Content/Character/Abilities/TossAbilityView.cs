using UnityEngine;

namespace Game
{
    public class TossAbilityView : MonoBehaviour
    {
        private static readonly int BlowUp = Animator.StringToHash("BlowUp");
        
        [SerializeField] private Animator _animator;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _tossAudioClip;
        [SerializeField] private ParticleSystem _tossVFX;
        
        private TossComponent _tossComponent;

        private void Awake()
        {
            _tossComponent = GetComponent<TossComponent>();
            _tossComponent.OnToss += OnToss;
        }

        private void OnDestroy() => 
            _tossComponent.OnToss -= OnToss;

        private void OnToss()
        {
            _animator.SetTrigger(BlowUp);
            _tossVFX.Play();
            _audioSource.PlayOneShot(_tossAudioClip);
        }
    }
}