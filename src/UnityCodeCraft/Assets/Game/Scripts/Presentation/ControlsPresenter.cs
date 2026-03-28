using System;

namespace Game.Gameplay
{
    public class ControlsPresenter : IControlsPresenter
    {
        private readonly SaveLoad _saveLoad;

        public ControlsPresenter(SaveLoad saveLoad) => 
            _saveLoad = saveLoad;

        public void Save(Action<bool, int> callback) => 
            _saveLoad.Save(callback);

        public void Load(string version, Action<bool, int> callback) => 
            _saveLoad.Load(version, callback);
    }
}