using System;
using UnityEngine;

namespace Game
{
  [Serializable]
  public class HealthComponent
  {
    [SerializeField] private int _current;
    
    public HealthComponent(int maxHealth)
    {
      Max = maxHealth;
      _current = maxHealth;
    }

    public int Max { get; }
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