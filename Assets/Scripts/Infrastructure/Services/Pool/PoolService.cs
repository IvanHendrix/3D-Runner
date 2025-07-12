using System.Collections.Generic;
using Logic.Level.InteractableObjects;
using UnityEngine;

namespace Infrastructure.Services.Pool
{
    public class PoolService : MonoBehaviour
    {
        public static PoolService Instance { get; private set; }

        [SerializeField] private Coin _coinPrefab;
        [SerializeField] private JumpObstacle _jumpObstaclePrefab;
        [SerializeField] private StrafeObstacle _strafeObstaclePrefab;
        [SerializeField] private PooledVFX _vfxPrefab;

        private ObjectPool<Coin> _coinPool;
        private ObjectPool<JumpObstacle> _jumpObstaclePool;
        private ObjectPool<StrafeObstacle> _strafeObstaclePool;
        private ObjectPool<PooledVFX> _vfxPool;

        private void Awake()
        {
            Instance = this;
            _coinPool = new ObjectPool<Coin>(_coinPrefab, 10, transform);
            _jumpObstaclePool = new ObjectPool<JumpObstacle>(_jumpObstaclePrefab, 5, transform);
            _strafeObstaclePool = new ObjectPool<StrafeObstacle>(_strafeObstaclePrefab, 5, transform);
            _vfxPool = new ObjectPool<PooledVFX>(_vfxPrefab, 5, transform);
        }

        public Coin GetCoin()
        {
            return _coinPool.Get();
        }

        public Obstacle GetRandomObstacleExcluding(ObstacleType? exclude)
        {
            List<ObstacleType> types = new() { ObstacleType.Jump, ObstacleType.Strafe };
            if (exclude.HasValue)
            {
                types.Remove(exclude.Value);
            }

            ObstacleType selected = types[Random.Range(0, types.Count)];
            return selected switch
            {
                ObstacleType.Jump => _jumpObstaclePool.Get(),
                ObstacleType.Strafe => _strafeObstaclePool.Get(),
                _ => throw new System.Exception("Unsupported ObstacleType")
            };
        }

        public void ReturnCoin(Coin coin)
        {
            _coinPool.Return(coin);
        }

        public void ReturnObstacle(Obstacle obstacle)
        {
            if (obstacle is JumpObstacle j)
            {
                _jumpObstaclePool.Return(j);
            }
            else if (obstacle is StrafeObstacle s)
            {
                _strafeObstaclePool.Return(s);
            }
        }

        public void PlayVFX(Vector3 position)
        {
            var vfx = _vfxPool.Get();
            vfx.transform.position = position;
            vfx.Play(() => _vfxPool.Return(vfx));
        }
    }
}