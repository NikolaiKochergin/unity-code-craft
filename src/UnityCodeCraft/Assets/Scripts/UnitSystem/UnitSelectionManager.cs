using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SampleGame
{
    public sealed class UnitSelectionManager : MonoBehaviour, IEnumerable<Unit>
    {
        public event Action<Unit> OnUnitAdded;
        public event Action<Unit> OnUnitRemoved;

        public int UnitCount => _selectedUnits.Count;

        [ShowInInspector, ReadOnly]
        private readonly HashSet<Unit> _selectedUnits = new();

        public IEnumerator<Unit> GetEnumerator()
        {
            return _selectedUnits.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        [Button]
        public void InvertUnit(Unit unit)
        {
            if (_selectedUnits.Remove(unit))
            {
                this.OnUnitRemoved?.Invoke(unit);
            }
            else
            {
                _selectedUnits.Add(unit);
                this.OnUnitAdded?.Invoke(unit);
            }
        }

        public void SelectUnits(IEnumerable<Unit> units)
        {
            this.ClearUnits();
            this.AddUnits(units);
        }

        public void AddUnits(IEnumerable<Unit> units)
        {
            foreach (Unit unit in units) 
                this.AddUnit(unit);
        }

        [Button]
        public void SelectUnit(Unit unit)
        {
            this.ClearUnits();
            this.AddUnit(unit);
        }

        [Button]
        public void AddUnit(Unit unit)
        {
            if (!unit)
                return;

            if (_selectedUnits.Contains(unit))
                return;

            _selectedUnits.Add(unit);
            this.OnUnitAdded?.Invoke(unit);
        }

        [Button]
        public void RemoveUnit(Unit unit)
        {
            if (_selectedUnits.Remove(unit))
                this.OnUnitRemoved?.Invoke(unit);
        }

        [Button]
        public void ClearUnits()
        {
            foreach (Unit unit in _selectedUnits)
                this.OnUnitRemoved?.Invoke(unit);

            _selectedUnits.Clear();
        }
        
        [Button]
        public void SelectAllUnits()
        {
            _selectedUnits.Clear();

            Unit[] units = FindObjectsOfType<Unit>();
            for (int i = 0, count = units.Length; i < count; i++)
                this.AddUnit(units[i]);
        }

        public void SelectUnit(Ray ray, bool additive = false)
        {
            if (!Physics.Raycast(ray, out RaycastHit hit) || !hit.transform.TryGetComponent(out Unit target)) 
                return;
            
            if (additive)
                this.InvertUnit(target);
            else
                this.SelectUnit(target);
        }
        
        public void SelectUnits(Vector3 startWorldPoint, Vector3 endWorldPoint, bool additive = false)
        {
            float minX = Mathf.Min(startWorldPoint.x, endWorldPoint.x);
            float maxX = Mathf.Max(startWorldPoint.x, endWorldPoint.x);
            float minZ = Mathf.Min(startWorldPoint.z, endWorldPoint.z);
            float maxZ = Mathf.Max(startWorldPoint.z, endWorldPoint.z);
            
            HashSet<Unit> units = new HashSet<Unit>();
            Unit[] allUnits = FindObjectsOfType<Unit>();
            for (int i = 0, count = allUnits.Length; i < count; i++)
            {
                Unit unit = allUnits[i];
                if (this.InRectangle(unit, minX, maxX, minZ, maxZ)) 
                    units.Add(unit);
            }

            if (additive)
                this.AddUnits(units);
            else
                this.SelectUnits(units);
        }
        
        
        private bool InRectangle(Unit unit, float minX, float maxX, float minZ, float maxZ)
        {
            Vector3 position = unit.transform.position;
            if (this.PointInRectangle(position, minX, maxX, minZ, maxZ))
                return true;

            const float HEIGHT_COEF = 2;
            float radius = unit.Scale;
            float height = unit.Height / HEIGHT_COEF;

            return this.PointInRectangle(position + new Vector3(radius, 0.0f, 0.0f), minX, maxX, minZ, maxZ) ||
                   this.PointInRectangle(position - new Vector3(radius, 0.0f, 0.0f), minX, maxX, minZ, maxZ) ||
                   this.PointInRectangle(position + new Vector3(0.0f, 0.0f, radius), minX, maxX, minZ, maxZ) ||
                   this.PointInRectangle(position - new Vector3(0.0f, 0.0f, radius), minX, maxX, minZ, maxZ) ||
                   this.PointInRectangle(position + new Vector3(0.0f, 0.0f, radius + height), minX, maxX, minZ, maxZ);
        }

        private bool PointInRectangle(Vector3 point, float minX, float maxX, float minZ, float maxZ)
        {
            return point.x >= minX && point.x <= maxX &&
                   point.z >= minZ && point.z <= maxZ;
        }
    }
}