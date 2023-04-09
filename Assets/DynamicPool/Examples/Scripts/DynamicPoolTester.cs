using UnityEngine;
using SG.Global.PoolSystem;

public class DynamicPoolTester : MonoBehaviour
    {
        [SerializeField] private TestPoolItem _poolItem1;
        [SerializeField] private TestPoolItem _poolItem2;
        [SerializeField] private ParticleSystem _poolParticleSystem;

        private Camera _mainCamera;

        private void Awake()
        {
            _mainCamera = Camera.main;
            DynamicPool.CreatePool(_poolItem1, 7);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    DynamicPool.Clear(_poolItem1);
                }
                else
                {
                    DynamicPool.Get(_poolItem1, GetSpawnPoint());
                }
            }
            
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    DynamicPool.Clear(_poolItem2);
                }
                else
                {
                    DynamicPool.Get(_poolItem2, GetSpawnPoint());
                }
            }
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    DynamicPool.Clear(_poolParticleSystem);
                }
                else
                {
                    DynamicPool.Get(_poolParticleSystem, GetSpawnPoint());
                }
            }
            
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                DynamicPool.ClearAll();
            }
        }

        private Vector3 GetSpawnPoint()
        {
            var mousePosition = Input.mousePosition;
            mousePosition.z = 10;
            var spawnPoint = _mainCamera.ScreenToWorldPoint(mousePosition);
            return spawnPoint;
        }
    }
