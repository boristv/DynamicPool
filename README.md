# DynamicPool

- Need Unity version 2021+
- Implemented using Unity ObjectPool
- Doesn't require the creation of additional objects on the scene
- Simplifies the use of the pool system.

## Installation

- Import unitypackage from releases
- If examples aren't needed, uncheck the folder "Examples"

## Instruction

Use the static class DynamicPool from namespace SG.GLobal.PoolSystem

- DynamicPool.Get - use to spawn object
- DynamicPool.CreatePool - use to prespawn objects
- DynamicPool.Clear - clear pool for one object
- DynamicPool.ClearAll - clear pool for all objects

For use this method object must realize IPoolable interface. 

Also you can use this methods to ParticleSystem. 
Then the necessary component will be added automatically and ParticleSystem will return to the pool when it is played to the end.

### IPoolable

- ReturnToPool - invoke this event to return object to pool
- GameObject - return gameObject or cached gameObject
- OnTakeFromPool - this method called when object was took from pool
- OnReturnToPool - this method called when object was returned to pool

### Editor

The available and spawned number of objects can be observed in the scene hierarchy.

![image](https://user-images.githubusercontent.com/22861315/229386548-911a2d49-6481-4c12-b47c-3377c11fff7d.png)

