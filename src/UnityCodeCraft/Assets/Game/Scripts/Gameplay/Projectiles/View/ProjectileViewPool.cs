using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Behaviour = Fusion.Behaviour;

namespace Game
{
    public sealed class ProjectileViewPool : Behaviour
    {
        [SerializeField] private Transform _container;
        [SerializeField] private Pool _pool = new();

        [Inject]
        public void Construct(IInstantiator instantiator) => 
            _pool._instantiator = instantiator;

        public ProjectileView Rent(Transform parent) => _pool.Rent(parent);

        public void Return(ProjectileView view)
        {
            if(view == null)
                return;

            view.transform.SetParent(_container, false);
            _pool.Return(view);
        }

        [Serializable]
        private sealed class Pool
        {
            internal IInstantiator _instantiator;

            private ProjectileView _prefab;

            private readonly Stack<ProjectileView> _available = new();

            public ProjectileView Rent(Transform parent)
            {
                ProjectileView view = _available.Count > 0 ? _available.Pop() : Create(parent);
                view.transform.SetParent(parent, false);
                view.gameObject.SetActive(true);
                return view;
            }

            public void Return(ProjectileView view)
            {
                view.gameObject.SetActive(false);
                _available.Push(view);
            }

            private ProjectileView Create(Transform parent)
            {
                ProjectileView view = _instantiator.InstantiatePrefabForComponent<ProjectileView>(_prefab, parent);
                view.name = _prefab.name;
                return view;
            }
        }
    }
}