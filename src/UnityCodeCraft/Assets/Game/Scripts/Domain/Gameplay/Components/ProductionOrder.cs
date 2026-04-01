using System.Collections.Generic;
using Game.Gameplay;
using Modules.Entities;
using UnityEngine;
using Zenject;

namespace SampleGame.Gameplay
{
    //Can be extended
    public sealed class ProductionOrder : MonoBehaviour, IComponentSerializer<List<string>>
    {
        ///Variable
        [SerializeField]
        private List<EntityConfig> _queue;

        public IReadOnlyList<EntityConfig> Queue
        {
            get { return _queue; }
            set { _queue = new List<EntityConfig>(value); }
        }

        private EntityCatalog _catalog;

        [Inject]
        public void Construct(EntityCatalog catalog) => 
            _catalog = catalog;

        public string Key => nameof(ProductionOrder);
        
        public List<string> Serialize()
        {
            List<string> queue = new(_queue.Count);
            foreach (EntityConfig entityConfig in _queue)
                queue.Add(entityConfig.Name);
            
            return queue;
        }

        public void Deserialize(List<string> data)
        {
            _queue = new List<EntityConfig>(data.Count);
            foreach (string entityName in data)
                if(_catalog.FindConfig(entityName, out EntityConfig entityConfig))
                    _queue.Add(entityConfig);
        }
    }
}