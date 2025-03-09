using System.Collections.Generic;
using UI;
using UnityEngine;

namespace Managers
{
    /// <summary>
    /// Manages obstacle placement and destruction.
    /// </summary>
    public class ObstacleManager : MonoBehaviour
    {
        private readonly List<GameObject> _placedObstacles = new List<GameObject>();
        private Timer _timer;

        private void Awake()
        {
            _timer = FindAnyObjectByType<Timer>();
        }

        private void OnEnable()
        {
            _timer.OnWaveEnded += DestroyAllObstacles;
        }

        private void OnDisable()
        {
            _timer.OnWaveEnded -= DestroyAllObstacles;
        }

        /// <summary>
        /// Registers a newly placed obstacle.
        /// </summary>
        public void RegisterObstacle(GameObject obstacle)
        {
            _placedObstacles.Add(obstacle);
        }

        /// <summary>
        /// Destroys all registered obstacles at the end of a wave.
        /// </summary>
        private void DestroyAllObstacles()
        {
            for (var indexObject = 0; indexObject < _placedObstacles.Count; indexObject++)
            {
                var obstacle = _placedObstacles[indexObject];
                if (obstacle != null)
                {
                    Destroy(obstacle);
                }
            }
            _placedObstacles.Clear();
        }
    }
}