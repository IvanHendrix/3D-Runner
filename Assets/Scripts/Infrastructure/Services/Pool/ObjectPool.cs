using System.Collections.Generic;
using UnityEngine;

namespace Infrastructure.Services.Pool
{
    public class ObjectPool<T> where T : MonoBehaviour, IPoolable
    {
        private readonly Queue<T> _pool = new();
        private readonly T _prefab;
        private readonly Transform _parent;

        public ObjectPool(T prefab, int initialCount, Transform parent)
        {
            _prefab = prefab;
            _parent = parent;

            for (int i = 0; i < initialCount; i++)
            {
                T obj = Object.Instantiate(_prefab, _parent);
                obj.gameObject.SetActive(false);
                _pool.Enqueue(obj);
            }
        }

        public T Get()
        {
            T obj = _pool.Count > 0 ? _pool.Dequeue() : Object.Instantiate(_prefab, _parent);
            obj.gameObject.SetActive(true);
            return obj;
        }

        public void Return(T obj)
        {
            obj.ResetObject();
            obj.gameObject.SetActive(false);
            _pool.Enqueue(obj);
        }
    }
}