using System.Collections.Generic;
using Modules.Entities;
using SampleGame.Gameplay;

namespace Game.Gameplay
{
    public class ProductionOrderComponentSerializer : IComponentSerializer<ProductionOrder, string[]>
    {
        private EntityCatalog _catalog;

        public ProductionOrderComponentSerializer(EntityCatalog catalog) => 
            _catalog = catalog;

        public string[] Serialize(ProductionOrder component)
        {
            string[] queue = new string[component.Queue.Count];
            for (int i = 0; i < queue.Length; i++) 
                queue[i] = component.Queue[i].Name;
            
            return queue;
        }

        public void Deserialize(ProductionOrder component, string[] data)
        {
            List<EntityConfig> queue = new(data.Length);
            foreach (string entityName in data)
                if(_catalog.FindConfig(entityName, out EntityConfig entityConfig))
                    queue.Add(entityConfig);
            
            component.Queue = queue;
        }
    }
}