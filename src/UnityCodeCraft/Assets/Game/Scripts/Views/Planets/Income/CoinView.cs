using System;
using UnityEngine;
using Zenject;

namespace Game.Views
{
    public class CoinView : MonoBehaviour
    {
        private CoinParticleView _coinParticle;

        [Inject]
        public void Construct(CoinParticleView coinParticle) => 
            _coinParticle = coinParticle;

        public void Show() => 
            gameObject.SetActive(true);
        
        public void Hide() =>
            gameObject.SetActive(false);

        public void ShowGatherEffect(Action onFinished)
        {
            gameObject.SetActive(false);
            _coinParticle.Emit(transform.position, onFinished);
        }
    }
}