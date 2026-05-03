using UnityEngine;

namespace Game
{
    public sealed class LookComponent : MonoBehaviour
    {
        public void Look(GameObject target)
        {
            Vector2 direction = target.transform.position - transform.position;
            Look(direction.x);
        }
        
        public void Look(float direction)
        {
            float angle = direction > 0 ? 0 : 180;
            transform.eulerAngles = new Vector3(0, angle, 0);
        }
    }
}