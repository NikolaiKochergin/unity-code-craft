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
        
        private TossAbility _tossAbility;

        private void Awake()
        {
            _tossAbility = GetComponent<TossAbility>();
            _tossAbility.OnToss += OnToss;
        }

        private void OnDestroy() => 
            _tossAbility.OnToss -= OnToss;

        private void OnToss()
        {
            _animator.SetTrigger(BlowUp);
            _tossVFX.Play();
            _audioSource.PlayOneShot(_tossAudioClip);
        }
    }
}