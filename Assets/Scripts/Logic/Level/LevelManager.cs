using System.Collections.Generic;
using Logic.Level.Road;
using UnityEngine;

namespace Logic.Level
{
    public class LevelManager : MonoBehaviour
    {
        [Header("Segment Settings")] 
        [SerializeField]
        private RoadSegmentSpawner _segmentPrefab;

        [SerializeField] private int _initialSegmentCount = 7;
        [SerializeField] private float _segmentLength = 10f;
        [SerializeField] private float _moveSpeed = 5f;

        [Header("Spawn")]
        [SerializeField] private Transform _spawnOrigin;

        private readonly Queue<RoadSegmentSpawner> _segmentPool = new();
        private readonly List<RoadSegmentSpawner> _activeSegments = new();

        private void Start()
        {
            InitPool();
            SpawnInitialSegments();
        }

        private void Update()
        {
            MoveSegments();
            HandleSegmentRecycling();
        }

        private void InitPool()
        {
            for (int i = 0; i < _initialSegmentCount; i++)
            {
                RoadSegmentSpawner segment = Instantiate(_segmentPrefab);
                segment.gameObject.SetActive(false);
                _segmentPool.Enqueue(segment);
            }
        }

        private void SpawnInitialSegments()
        {
            for (int i = 0; i < _initialSegmentCount; i++)
            {
                SpawnSegment(i);
            }
        }

        private void MoveSegments()
        {
            float moveStep = _moveSpeed * Time.deltaTime;

            foreach (var segment in _activeSegments)
            {
                segment.transform.position += Vector3.back * moveStep;
            }
        }

        private void HandleSegmentRecycling()
        {
            RoadSegmentSpawner first = _activeSegments[0];
            if (first.transform.position.z < -_segmentLength)
            {
                RecycleSegment(first);
                _activeSegments.RemoveAt(0);
                SpawnSegment();
            }
        }

        private void SpawnSegment(int index = -1)
        {
            RoadSegmentSpawner segment = _segmentPool.Dequeue();

            float z = index >= 0
                ? _spawnOrigin.position.z + index * _segmentLength
                : _activeSegments[^1].transform.position.z + _segmentLength;

            segment.transform.position = new Vector3(0f, _spawnOrigin.position.y, z);
            segment.gameObject.SetActive(true);
            _activeSegments.Add(segment);

            segment.SpawnContent();
        }

        private void RecycleSegment(RoadSegmentSpawner segment)
        {
            segment.ClearContent();
            segment.gameObject.SetActive(false);
            _segmentPool.Enqueue(segment);
        }
    }
}