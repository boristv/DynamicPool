namespace SG.Global.PoolSystem
{
    public interface IPoolable
    {
        void OnTakeFromPool() {}
        void OnReturnToPool() {}
    }
}
