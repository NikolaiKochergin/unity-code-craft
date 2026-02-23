using System;
using UnityEngine;

namespace Game
{
  [Serializable]
  public class HealthComponent : IHealth
  {
    [SerializeField] private int _current;
    
    private HealthConfig _config;

    public void Setup(HealthConfig config)
    {
      _config = config;
      _current = config.Max;
    }

    public int Max => _config.Max;
    public int Current =>  _current;
    public bool IsAlive => Current > 0;

    public event Action<int> OnChanged;
    public event Action OnDied;

    public void TakeDamage(int damage)
    {
      if (damage <= 0 || !IsAlive)
        return;

      _current = Mathf.Clamp(_current - damage, 0, Max);
      OnChanged?.Invoke(_current);
      
      if(Current <= 0)
        OnDied?.Invoke();
    }

    public void Restore()
    {
      _current = Max;
      OnChanged?.Invoke(_current);
    }
  }
}