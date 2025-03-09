using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    /// <summary>
    /// Implements a basic object pooling system to optimize performance
    /// by reusing GameObjects instead of instantiating and destroying them repeatedly.
    /// </summary>
    public class PoolingSystem : MonoBehaviour
    {
        private readonly Dictionary<string, Queue<GameObject>> _enemyPools = new Dictionary<string, Queue<GameObject>>();
        [SerializeField] private GameObject pool;

        /// <summary>
        /// Initializes a pool for a given prefab, creating a predefined number of inactive instances.
        /// </summary>
        /// <param name="prefab">The GameObject to be pooled.</param>
        /// <param name="poolSize">The initial number of instances to be created.</param>
        public void InitializePool(GameObject prefab, int poolSize)
        {
            if (prefab == null || poolSize <= 0) return;

            var prefabName = prefab.name;
            if (!_enemyPools.ContainsKey(prefabName))
            {
                _enemyPools[prefabName] = new Queue<GameObject>();
            }

            for (var i = 0; i < poolSize; i++)
            {
                var enemy = Instantiate(prefab, pool.transform);
                enemy.SetActive(false);
                _enemyPools[prefabName].Enqueue(enemy);
            }
        }

        /// <summary>
        /// Retrieves an object from the pool if available; otherwise, instantiates a new one.
        /// </summary>
        /// <param name="prefab">The GameObject to retrieve from the pool.</param>
        /// <param name="spawnPos">The position where the object should be placed.</param>
        /// <param name="rotation">The rotation to apply to the object.</param>
        /// <returns>A GameObject ready for use.</returns>
        public GameObject AskForGameObject(GameObject prefab, Vector3 spawnPos, Quaternion rotation)
        {
            if (prefab == null) return null;

            var prefabName = prefab.name;
            GameObject enemy;

            if (_enemyPools.ContainsKey(prefabName) && _enemyPools[prefabName].Count > 0)
            {
                enemy = _enemyPools[prefabName].Dequeue();
            }
            else
            {
                enemy = Instantiate(prefab);
            }

            enemy.transform.position = spawnPos;
            enemy.transform.rotation = rotation;
            enemy.SetActive(true);

            return enemy;
        }
    }
}
