using System.Collections.Generic;
using UnityEngine;

namespace Logic.Level.Road
{
    public class RoadPool : MonoBehaviour
    {
        public GameObject prefab;
        public int initialSize = 5;

        private Queue<GameObject> pool = new Queue<GameObject>();

        public void Initialize()
        {
            for (int i = 0; i < initialSize; i++)
            {
                GameObject obj = Instantiate(prefab, transform);
                obj.SetActive(false);
                pool.Enqueue(obj);
            }
        }

        public GameObject Get()
        {
            if (pool.Count > 0)
            {
                GameObject obj = pool.Dequeue();
                obj.SetActive(true);
                return obj;
            }

            return Instantiate(prefab);
        }

        public void Return(GameObject obj)
        {
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }
}