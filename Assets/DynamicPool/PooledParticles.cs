using System;
using UnityEngine;

namespace SG.Global
{
    [RequireComponent(typeof(ParticleSystem))]
    public class PooledParticles : MonoBehaviour, IPoolable
    {
        public event Action ReturnToPool;
        public GameObject GameObject { get; private set; }
        public ParticleSystem ParticleSystem { get; set; }

        private void Awake()
        {
            GameObject = gameObject;
        }
        
        public void OnTakeFromPool()
        {
            ParticleSystem.Play();
            var main = ParticleSystem.main;
            main.stopAction = ParticleSystemStopAction.Callback;
        }

        private void OnParticleSystemStopped()
        {
            ReturnToPool?.Invoke();
        }

        public void OnReturnToPool()
        {
            ParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }
}
