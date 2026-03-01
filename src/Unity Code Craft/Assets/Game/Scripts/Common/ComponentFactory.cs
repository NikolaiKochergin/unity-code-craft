using UnityEngine;

namespace Game
{
    public abstract class ComponentFactory<TComponent> : MonoBehaviour where TComponent : Component
    {
        [SerializeField] private TComponent _prefab;
        [SerializeField] private Transform _container;

        public TComponent Create()
        {
            TComponent newComponent = Instantiate(_prefab, _container);
            Inject(newComponent);
            return newComponent;
        }

        protected virtual void Inject(TComponent component) { }
    }
}