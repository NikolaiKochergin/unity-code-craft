using System.Collections.Generic;
using SampleGame;
using UnityEngine;

namespace Game
{
    public sealed class PanelViewPresenter : MonoBehaviour
    {
        [SerializeField] private PanelView _panelView;
        [SerializeField] private UnitCardsCatalog _unitCardsCatalog;

        private readonly List<UnitCardVewPresenter> _presenters = new();

        private void Start()
        {
            foreach (UnitCardConfig config in _unitCardsCatalog.Cards)
            {
                UnitCardView card = _panelView.GetCard();
                _presenters.Add(new UnitCardVewPresenter(card, config));
            }
        }

        private void Update()
        {
            foreach (UnitCardVewPresenter presenter in _presenters) 
                presenter.Update();
        }
    }
}