using SampleGame;
using TMPro;
using UnityEngine;

namespace Game
{
    public sealed class PanelView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _energyText;
        [SerializeField] private UnitCardView _unitCardPrefab;
        [SerializeField] private Transform _content;
        
        public string EnergyAmountText
        {
            get => _energyText.text;
            set => _energyText.text = value;
        }

        public UnitCardView GetCard() => 
            Instantiate(_unitCardPrefab, _content);
    }
}