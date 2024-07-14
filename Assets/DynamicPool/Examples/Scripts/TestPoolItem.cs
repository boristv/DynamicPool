using System;
using System.Collections;
using UnityEngine;
using SG.Global.PoolSystem;

public class TestPoolItem : MonoBehaviour, IPoolable
{
    [SerializeField] private float _existTime = 5f;

    public void OnTakeFromPool()
    {
        StartCoroutine(ReleaseWithDelay());
    }

    public void OnReturnToPool()
    {
        StopAllCoroutines();
    }

    private IEnumerator ReleaseWithDelay()
    {
        yield return new WaitForSeconds(_existTime);
        DynamicPool.Return(this);
    }
}