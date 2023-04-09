using System;
using UnityEngine;

namespace SG.Global
{
    public interface IPoolable
    {
        event Action ReturnToPool;
        GameObject GameObject { get; }
        void OnTakeFromPool();
        void OnReturnToPool();
    }
}
