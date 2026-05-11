using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public class PickUpViewInstaller : GameEntityInstaller, IGameEntityDispose
    {
        [SerializeField] public ParticleSystem _pickUpParticles;
        [SerializeField] public AudioSource _audioSource;
        [SerializeField] public AudioClip _pickUpSound;
        
        private Subscription _subscription;

        public override void Install(IGameEntity entity)
        {
            _subscription = entity.GetValue(GameEntityAPI.CollectedEvent)
                .Subscribe(() =>
                {
                    gameObject.SetActive(false);
                    _pickUpParticles.Play();
                    _audioSource.PlayOneShot(_pickUpSound);
                });
        }
        
        public void Dispose(IGameEntity entity) => 
            _subscription.Dispose();
    }
}