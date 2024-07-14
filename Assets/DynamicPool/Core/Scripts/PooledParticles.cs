using System;
using UnityEngine;

namespace SG.Global.PoolSystem
{
    [RequireComponent(typeof(ParticleSystem))]
    public class PooledParticles : MonoBehaviour, IPoolable
    {
        public ParticleSystem ParticleSystem { get; set; }
        
        public void OnTakeFromPool()
        {
            ParticleSystem.Play();
            var main = ParticleSystem.main;
            main.stopAction = ParticleSystemStopAction.Callback;
        }

        private void OnParticleSystemStopped()
        {
            DynamicPool.Return(this);
        }

        public void OnReturnToPool()
        {
            ParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }
}
