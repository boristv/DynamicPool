using System.Collections.Generic;
using UnityEngine;

namespace SG.Global
{
    public class PoolContainer
    {
        private Transform _mainContainer;
        private Dictionary<int, SubContainer> _subContainers = new();
        
        public SubContainer GetSubContainer(int id, string name)
        {
            if (_mainContainer == null)
            {
                _mainContainer = new GameObject()
                {
                    name = "Pool Container"
                }.transform;
                Object.DontDestroyOnLoad(_mainContainer);
            }
            
            if (_subContainers.TryGetValue(id, out var subContainer)) 
                return subContainer;
            
            subContainer = new SubContainer(name, _mainContainer);
            _subContainers[id] = subContainer;

            return subContainer;
        }

        public void RemoveSubContainer(int id)
        {
            if (_subContainers.TryGetValue(id, out var subContainer))
            {
                Object.Destroy(subContainer.Transform.gameObject);
                _subContainers.Remove(id);
            }
        }
        
        public void RemoveAllContainers()
        {
            foreach (var subContainer in _subContainers)
            {
                Object.Destroy(subContainer.Value.Transform.gameObject);
            }

            _subContainers.Clear();
        }
        
        public void UpdateSubContainerName(int id, int availableCount, int maxCount)
        {
#if UNITY_EDITOR
            if (_subContainers.TryGetValue(id, out var subContainer))
            {
                subContainer.UpdateSubContainerName(availableCount, maxCount);
            }
#endif
        }
    }
    
    public class SubContainer
    {
        public Transform Transform { get; }

        private readonly string _baseName;

        public SubContainer(string name, Transform parent)
        {
            _baseName = name;
            Transform = new GameObject()
            {
                name = $"{_baseName} Pool"
            }.transform;
            Transform.SetParent(parent);
        }

        public void UpdateSubContainerName(int availableCount, int maxCount)
        {
            Transform.name = $"{_baseName} Pool {availableCount}/{maxCount}";
        }
    }
}
