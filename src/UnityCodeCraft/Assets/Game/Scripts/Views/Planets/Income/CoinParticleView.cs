using System;
using Modules.UI;
using UnityEngine;

namespace Game.Views
{
    public class CoinParticleView : MonoBehaviour
    {
        [SerializeField] private ParticleAnimator _particleAnimator;
        [SerializeField] private Transform _particlesTarget;

        public void Emit(Vector3 sourcePosition, Action onFinished) =>
            _particleAnimator
                .Emit(
                    sourcePosition, 
                    _particlesTarget.position,
                    onFinished: onFinished);
    }
}