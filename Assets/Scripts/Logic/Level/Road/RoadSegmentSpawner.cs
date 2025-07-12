using System.Collections.Generic;
using System.Linq;
using Infrastructure.Services.Pool;
using Logic.Level.InteractableObjects;
using UnityEngine;

namespace Logic.Level.Road
{
    public class RoadSegmentSpawner : MonoBehaviour
    {
        [SerializeField] private Transform[] _spawnPoints;

        private readonly List<MonoBehaviour> _spawnedObjects = new();

        private ObstacleType? _lastObstacleType = null;

        public void SpawnContent()
        {
            int pointCount = _spawnPoints.Length;
            bool[] occupied = new bool[pointCount];

            for (int i = 0; i < pointCount; i++)
            {
                if (occupied[i])
                {
                    continue;
                }

                if (ShouldSpawnObstacle())
                {
                    var obstacle = PoolService.Instance.GetRandomObstacleExcluding(_lastObstacleType);
                    int spawnIndex = GetSpawnIndexForObstacle(obstacle, i, occupied);

                    if (spawnIndex == -1)
                    {
                        continue;
                    }

                    PlaceObstacleAndCoins(obstacle, spawnIndex, occupied);
                    _lastObstacleType = obstacle.Type;
                    continue;
                }

                TryPlaceCoinAt(i);
            }

            _lastObstacleType = null;
        }

        private bool ShouldSpawnObstacle()
        {
            return Random.value < 0.6f;
        }

        private int GetSpawnIndexForObstacle(Obstacle obstacle, int fallbackIndex, bool[] occupied)
        {
            if (obstacle.Type == ObstacleType.Strafe)
                return GetValidSideIndex(fallbackIndex, occupied);

            return fallbackIndex;
        }

        private void PlaceObstacleAndCoins(Obstacle obstacle, int index, bool[] occupied)
        {
            PlaceObject(obstacle, index);
            occupied[index] = true;

            switch (obstacle.Type)
            {
                case ObstacleType.Strafe:
                    ApplyStrafeOffset(obstacle, index);
                    int opposite = GetOppositeSideIndex(index);
                    
                    if (IsValidIndex(opposite) && !occupied[opposite])
                    {
                        TryPlaceCoinIfFree(opposite);
                        occupied[opposite] = true;
                    }
                    else
                    {
                        TryPlaceCoinIfFree(index);
                    }
                    break;

                case ObstacleType.Jump:
                    TryPlaceCoinIfFree(index, Vector3.up * 1.5f);
                    break;
            }
        }

        private void ApplyStrafeOffset(Obstacle obstacle, int index)
        {
            float offsetX = index < _spawnPoints.Length / 2 ? -2.5f : 2.5f;
            obstacle.transform.localPosition += Vector3.right * offsetX;
        }

        private int GetOppositeSideIndex(int strafeIndex)
        {
            if (strafeIndex < _spawnPoints.Length / 2 && IsValidIndex(strafeIndex + 1))
            {
                return strafeIndex + 1;
            }

            if (strafeIndex >= _spawnPoints.Length / 2 && IsValidIndex(strafeIndex - 1))
            {
                return strafeIndex - 1;
            }
            return -1;
        }

        private void TryPlaceCoinIfFree(int index, Vector3 offset = default)
        {
            if (!IsValidIndex(index) || _spawnPoints[index] == null)
            {
                return;
            }

            bool isFree = _spawnedObjects.All(obj => obj.transform.position != _spawnPoints[index].position + offset);

            if (!isFree)
            {
                return;
            }

            var coin = PoolService.Instance.GetCoin();
            PlaceObject(coin, index, offset);
        }

        private void TryPlaceCoinAt(int index, Vector3 offset = default)
        {
            if (!IsValidIndex(index) || _spawnPoints[index] == null)
            {
                return;
            }

            var coin = PoolService.Instance.GetCoin();
            PlaceObject(coin, index, offset);
        }

        private void PlaceObject(MonoBehaviour obj, int index, Vector3 offset = default)
        {
            obj.transform.position = _spawnPoints[index].position + offset;
            obj.transform.SetParent(transform);
            _spawnedObjects.Add(obj);
        }

        private int GetValidSideIndex(int center, bool[] occupied)
        {
            List<int> options = new();
            int left = center - 1;
            int right = center + 1;

            if (IsValidIndex(left) && !occupied[left]) options.Add(left);
            if (IsValidIndex(right) && !occupied[right]) options.Add(right);

            return options.Count > 0 ? options[Random.Range(0, options.Count)] : -1;
        }

        private bool IsValidIndex(int index)
        {
            return index >= 0 && index < _spawnPoints.Length;
        }

        public void ClearContent()
        {
            foreach (var obj in _spawnedObjects)
            {
                obj.transform.SetParent(null);

                switch (obj)
                {
                    case Coin coin:
                        coin.ResetObject();
                        PoolService.Instance.ReturnCoin(coin);
                        break;

                    case Obstacle obstacle:
                        obstacle.ResetObject();
                        PoolService.Instance.ReturnObstacle(obstacle);
                        break;
                }
            }

            _spawnedObjects.Clear();
        }
    }
}