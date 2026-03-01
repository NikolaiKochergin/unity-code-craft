using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public abstract class ComponentPool<TComponent> : MonoBehaviour where TComponent : Component
    {
        [SerializeField] private ComponentFactory<TComponent> _factory;
        [SerializeField] private int _initialCapacity = 10;
        [SerializeField] private bool _isExtensible;

        private readonly Stack<TComponent> _pool = new();

        public void Awake()
        {
            for (int i = 0; i < _initialCapacity; i++)
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
            TComponent newComponent = _factory.Create();
            newComponent.gameObject.SetActive(false);

            return newComponent;
        }
    }
}