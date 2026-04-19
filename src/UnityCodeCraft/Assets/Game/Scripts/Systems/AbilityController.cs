using UnityEngine;

namespace Game
{
    public class AbilityController : MonoBehaviour
    {
        [SerializeField] private GameObject _character;
        
        private Character _tossComponent;
        private Character _pushComponent;

        private void Awake()
        {
            _tossComponent = _character.GetComponent<Character>();
            _pushComponent = _character.GetComponent<Character>();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
                _pushComponent.Push();
            
            if (Input.GetMouseButtonDown(1))
                _tossComponent.Toss();
        }
    }
}
