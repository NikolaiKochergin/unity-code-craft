using System.Collections.Generic;
using UnityEngine;

namespace Game
{
  public class ComponentPool<TComponent> where TComponent : Component
  {
    private readonly Stack<TComponent> _pool = new();
    private readonly TComponent _prefab;
    private readonly Transform _parent;
    private readonly bool _isExtensible;

    public ComponentPool(
      TComponent prefab, 
      int initialCapacity = 10, 
      Transform parent = null, 
      bool isExtensible  = true)
    {
      _isExtensible = isExtensible;
      _parent = parent;
      _prefab = prefab;

      for (int i = 0; i < initialCapacity; i++) 
        _pool.Push(NewComponent());
    }

    public TComponent Get() =>
      _pool.Count > 0
        ? _pool.Pop()
        : _isExtensible
          ? NewComponent()
          : null;

    public bool TryGet(out TComponent component)
    {
      component = Get();
      return component;
    }

    public void Return(TComponent component)
    {
      if (_pool.Contains(component))
      {
        Debug.LogWarning($"Pool already contains component: {component.name}");
        return;
      }
      
      _pool.Push(component);
    }


    private TComponent NewComponent()
    {
      TComponent newComponent = Object.Instantiate(_prefab, _parent);
      newComponent.gameObject.SetActive(false);
      
      return newComponent;
    }
  }
}