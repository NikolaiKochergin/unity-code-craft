using System;
using UnityEngine;

namespace Game
{
  public class ExplosionParticles : MonoBehaviour
  {
    [SerializeField] private ParticleSystem _particleSystem;
    
    private Action<ExplosionParticles> _onStopped;

    public void Play(Action<ExplosionParticles> onStopped)
    {
      _particleSystem.Play();
      _onStopped = onStopped;
    }

    private void OnParticleSystemStopped()
    {
      _onStopped?.Invoke(this);
      _onStopped = null;
    }

#if UNITY_EDITOR
    private void Reset()
    {
      _particleSystem = GetComponent<ParticleSystem>();
      if(_particleSystem == null)
        Debug.LogError("No ParticleSystem found on " + gameObject.name);
    }
#endif
  }
}