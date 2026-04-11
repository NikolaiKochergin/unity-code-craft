using UnityEngine;

namespace Game
{
    public class TossRequestComponent : MonoBehaviour
    {
        interface IAction
        {
            public void Invoke();
        }
        
        public interface ICondition
        {
            bool Evaluate();
        }
        
        
        
        public void Toss()
        {
            
        }
    }
}