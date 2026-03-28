using System;
using UnityEngine;

namespace Game.Gameplay
{
    public class SaveLoad
    {
        public void Save(Action<bool, int> callback)
        {
            Debug.Log("Save");
        }

        public void Load(string version, Action<bool, int> callback)
        {
            Debug.Log("Load:  " + version);
        }
    }
}