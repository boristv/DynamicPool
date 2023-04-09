using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace SG.Global
{
    public static class DynamicPool
    {
        private static Dictionary<int, ObjectPool<IPoolable>> _objectPools = new();
        
        private static PoolContainer _poolContainer;
        private static PoolContainer PoolContainer => _poolContainer ??= new PoolContainer();

        private static int GetID<T>(T prefab) where T : Object => prefab.GetInstanceID();
        
        private static ObjectPool<IPoolable> GetPool<T>(T prefab) where T : Object, IPoolable
        {
            var id = GetID(prefab);

            if (_objectPools.TryGetValue(id, out var pool)) 
                return pool;
            
            pool = new ObjectPool<IPoolable>(
                () => CreatePooledItem(prefab),
                OnTakeFromPool,
                item => OnReturnedToPool(prefab, item),
                item => OnDestroyPoolObject(prefab, item));
            _objectPools[id] = pool;

            return pool;
        }
        
        private static ObjectPool<IPoolable> GetParticlesPool(ParticleSystem particlesPrefab)
        {
            var id = GetID(particlesPrefab);

            if (_objectPools.TryGetValue(id, out var pool)) 
                return pool;
            
            pool = new ObjectPool<IPoolable>(
                () => CreatePooledParticle(particlesPrefab),
                OnTakeFromPool,
                item => OnReturnedToPool(particlesPrefab, item),
                item => OnDestroyPoolObject(particlesPrefab, item));
            _objectPools[id] = pool;

            return pool;
        }

        private static SubContainer GetSubContainer<T>(T prefab) where T : Object
        {
            var id = GetID(prefab);

            var subContainer = PoolContainer.GetSubContainer(id, prefab.name);

            return subContainer;
        }

        public static void CreatePool<T>(T prefab, int count) where T : Object, IPoolable
        {
            Clear(prefab);
            var pool = GetPool(prefab);
            CreateReleasedPool(prefab, pool, count);
        }
        
        public static void CreatePool(ParticleSystem particlesPrefab, int count)
        {
            Clear(particlesPrefab);
            var pool = GetParticlesPool(particlesPrefab);
            CreateReleasedPool(particlesPrefab, pool, count);
        }
        
        private static void CreateReleasedPool<T>(T prefab, ObjectPool<IPoolable> pool, int count) where T : Object
        {
            var items = new List<IPoolable>();
            for (var i = 0; i < count; i++)
            {
                items.Add(pool.Get());
            }
            foreach (var item in items)
            {
                pool.Release(item);
            }
            UpdateSubContainerName(prefab);
        }

        public static T Get<T>(T prefab, Vector3 position, Transform parent = null) where T : Object, IPoolable
        {
            var pool = GetPool(prefab);

            var poolItem = (T) pool.Get();

            if (parent != null) poolItem.GameObject.transform.SetParent(parent);
            poolItem.GameObject.transform.position = position;

            ItemGot(prefab, poolItem, pool);

            return poolItem;
        }
        
        public static ParticleSystem Get(ParticleSystem particlesPrefab, Vector3 position, Transform parent = null)
        {
            var pool = GetParticlesPool(particlesPrefab);

            var poolItem = (PooledParticles) pool.Get();

            if (parent != null) poolItem.GameObject.transform.SetParent(parent);
            poolItem.GameObject.transform.position = position;

            ItemGot(particlesPrefab, poolItem, pool);

            return poolItem.ParticleSystem;
        }
        
        private static void ItemGot<T>(T prefab, IPoolable poolItem, ObjectPool<IPoolable> pool) where T : Object
        {
            poolItem.ReturnToPool -= ReturnToPool;
            poolItem.ReturnToPool += ReturnToPool;

            void ReturnToPool()
            {
                poolItem.ReturnToPool -= ReturnToPool;
                pool.Release(poolItem);
                UpdateSubContainerName(prefab);
            }

            UpdateSubContainerName(prefab);
        }

        private static void Clear(int id)
        {
            if (_objectPools.TryGetValue(id, out var pool))
            {
                pool.Clear();
                _objectPools.Remove(id);
            }

            PoolContainer.RemoveSubContainer(id);
        }

        public static void Clear<T>(T prefab) where T : Object, IPoolable
        {
            var id = GetID(prefab);
            Clear(id);
        }
        
        public static void Clear(ParticleSystem particlesPrefab)
        {
            var id = GetID(particlesPrefab);
            Clear(id);
        }

        public static void ClearAll()
        {
            foreach (var pool in _objectPools)
            {
                pool.Value.Clear();
            }

            PoolContainer.RemoveAllContainers();
        }

        private static IPoolable CreatePooledItem<T>(T prefab) where T : Object, IPoolable
        {
            var subContainer = GetSubContainer(prefab);

            var poolItem = Object.Instantiate(prefab, subContainer.Transform);

            UpdateSubContainerName(prefab);

            return poolItem;
        }
        
        private static PooledParticles CreatePooledParticle(ParticleSystem particlesPrefab)
        {
            var subContainer = GetSubContainer(particlesPrefab);

            var pooledParticles = Object.Instantiate(particlesPrefab, subContainer.Transform);
            var poolItem = pooledParticles.gameObject.AddComponent<PooledParticles>();
            poolItem.ParticleSystem = pooledParticles;

            UpdateSubContainerName(particlesPrefab);

            return poolItem;
        }

        private static void OnReturnedToPool<T>(T prefab, IPoolable poolItem) where T : Object
        {
            poolItem.GameObject.SetActive(false);
            poolItem.GameObject.transform.SetParent(GetSubContainer(prefab).Transform);
            poolItem.OnReturnToPool();
        }

        private static void OnTakeFromPool(IPoolable poolItem)
        {
            poolItem.GameObject.SetActive(true);
            poolItem.OnTakeFromPool();
        }

        private static void OnDestroyPoolObject<T>(T prefab, IPoolable poolItem) where T : Object
        {
            Object.Destroy(poolItem.GameObject);
            UpdateSubContainerName(prefab);
        }

        private static void UpdateSubContainerName<T>(T prefab) where T : Object
        {
#if UNITY_EDITOR
            var id = GetID(prefab);
            if (_objectPools.TryGetValue(id, out var pool))
            {
                PoolContainer.UpdateSubContainerName(id, pool.CountInactive, pool.CountAll);
            }
#endif
        }
    }
}