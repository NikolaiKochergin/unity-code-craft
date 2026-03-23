using System;
using Modules.UI;
using UnityEngine;

namespace Game.Views
{
    public class CoinParticleView : MonoBehaviour
    {
        [SerializeField] private ParticleAnimator _particleAnimator;
        [SerializeField] private Transform _targetTransform;

        public void ShowFrom(Vector3 sourcePosition, Action onFinished) =>
            _particleAnimator.Emit(
                from: sourcePosition,
                to: _targetTransform.position,
                onFinished: onFinished);
    }
}