using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    /// <summary>
    /// Handles enemy spawning in waves using an object pooling system.
    /// </summary>
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private Settings sets; 
        [SerializeField] private GameManager manager;
        [SerializeField] private PoolingSystem poolingSystem; 

        private void Start()
        {
            manager = FindAnyObjectByType<GameManager>();

            // Determine the maximum number of each enemy type across all waves.
            int maxTiny = GetMaxFromWaves(waves => waves.tinyEnemies);
            int maxMedium = GetMaxFromWaves(waves => waves.mediumEnemies);
            int maxTank = GetMaxFromWaves(waves => waves.tankEnemies);

            // Initialize object pools for enemy types.
            poolingSystem.InitializePool(sets.tinyPrefab, maxTiny);
            poolingSystem.InitializePool(sets.mediumPrefab, maxMedium);
            poolingSystem.InitializePool(sets.tankPrefab, maxTank);
        }

        private void OnEnable()
        {
            manager.OnStartWaves += Spawn;
        }

        private void OnDisable()
        {
            manager.OnStartWaves -= Spawn;
        }

        /// <summary>
        /// Starts spawning enemies for the given wave.
        /// </summary>
        /// <param name="waveCount">The index of the wave to spawn.</param>
        private void Spawn(int waveCount)
        {
            StartCoroutine(SpawnEnumerator(waveCount));
        }

        /// <summary>
        /// Coroutine that spawns enemies with a delay between spawns.
        /// </summary>
        /// <param name="waveC">The index of the wave count.</param>
        private IEnumerator SpawnEnumerator(int waveC)
        {
            if (waveC >= sets.waves.Length) yield break;

            WaveSettings waveSettings = sets.waves[waveC]; 
            List<GameObject> enemiesToSpawn = new List<GameObject>();

            // Add tiny enemies to spawn list.
            for (var i = 0; i < waveSettings.tinyEnemies; i++)
                enemiesToSpawn.Add(sets.tinyPrefab);
    
            // Add medium enemies to spawn list.
            for (var i = 0; i < waveSettings.mediumEnemies; i++)
                enemiesToSpawn.Add(sets.mediumPrefab);
    
            // Add tank enemies to spawn list.
            for (var i = 0; i < waveSettings.tankEnemies; i++)
                enemiesToSpawn.Add(sets.tankPrefab);

            // Randomize enemy spawn order.
            Shuffle(enemiesToSpawn);

            // Spawn enemies sequentially with a delay.
            foreach (var enemyPrefab in enemiesToSpawn)
            {
                poolingSystem.AskForGameObject(enemyPrefab, sets.posToSpawn, Quaternion.identity);
                yield return new WaitForSeconds(sets.timeBetweenSpawns);
            }
        }

        /// <summary>
        /// Shuffles a list of GameObjects using Fisher-Yates algorithm.
        /// </summary>
        /// <param name="list">The list to shuffle.</param>
        private static void Shuffle(IList<GameObject> list)
        {
            var rng = new System.Random();
            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = rng.Next(n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }
        }

        /// <summary>
        /// Gets the maximum number of a specific enemy type across all waves.
        /// </summary>
        /// <param name="selector">Function to select enemy count from a wave.</param>
        /// <returns>The highest count of the selected enemy type.</returns>
        private int GetMaxFromWaves(Func<WaveSettings, int> selector)
        {
            int max = 0;
            foreach (var wave in sets.waves)
            {
                int value = selector(wave);
                if (value > max) max = value;
            }
            return max;
        }
    }

    /// <summary>
    /// Configuration settings for the spawner.
    /// </summary>
    [Serializable]
    public struct Settings
    {
        public Vector3 posToSpawn; 
        public WaveSettings[] waves; 
        public GameObject tinyPrefab; 
        public GameObject mediumPrefab;
        public GameObject tankPrefab;
        public float timeBetweenSpawns;
    }

    /// <summary>
    /// Defines the enemy count per type for a wave.
    /// </summary>
    [Serializable]
    public struct WaveSettings
    {
        public int tinyEnemies;
        public int mediumEnemies;
        public int tankEnemies;
    }
}
