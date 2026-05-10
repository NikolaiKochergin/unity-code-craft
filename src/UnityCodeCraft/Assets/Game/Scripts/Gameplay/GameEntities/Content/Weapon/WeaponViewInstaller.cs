using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public class WeaponViewInstaller : SceneEntityInstaller<IGameEntity>
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _fireSound;
        [SerializeField] private ParticleSystem _fireParticles;
        
        public override void Install(IGameEntity weapon) =>
            weapon
                .GetValue(GameEntityAPI.FireCommand)
                .AddAction(() => _audioSource.PlayOneShot(_fireSound))
                .AddAction(() => _fireParticles.Play());
    }
}