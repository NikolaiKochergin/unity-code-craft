using UnityEngine;

namespace Game
{
    public class AbilityController : MonoBehaviour
    {
        [SerializeField] private GameObject _character;
        
        private Character _attacker;

        private void Awake()
        {
            _attacker = _character.GetComponent<Character>();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
                _attacker.Push();
            
            if (Input.GetMouseButtonDown(1))
                _attacker.Toss();
        }
    }
}
