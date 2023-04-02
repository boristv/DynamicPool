using System;
using System.Collections;
using SG.Global;
using UnityEngine;

public class TestPoolItem : MonoBehaviour, IPoolable
{
    public event Action ReturnToPool;

    [SerializeField] private float _existTime = 5f;

    public GameObject GameObject => gameObject;

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
        ReturnToPool?.Invoke();
    }
}