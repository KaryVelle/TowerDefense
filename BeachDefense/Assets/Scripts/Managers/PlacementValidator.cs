using System;
using System.Collections.Generic;
using UI;
using UnityEngine;

namespace Managers
{
    /// <summary>
    /// Manages the placement of obstacles within the game environment.
    /// Handles obstacle selection, preview positioning, rotation, and placement.
    /// </summary>
    public class PlacementManager : MonoBehaviour
    {
        /// <summary>
        /// Represents data related to a specific obstacle.
        /// </summary>
        [System.Serializable]
        public class ObstacleData
        {
            public GameObject prefab; 
            public int maxCount; 
            public int currentCount = 0; 
            public bool canPlace;
        }

        public List<ObstacleData> obstacles;
        public LayerMask placementLayer;

        private int _selectedObstacleIndex = -1;
        private GameObject _previewObject;
        [SerializeField] private Camera cam;
        private Vector3 _definitivePos;
        private Quaternion _actualRotation;
        private int _rotationIndex = 0;
        
        private ObstacleManager _obstacleManager;
        private Timer _timer;

        private void Awake()
        {
            _obstacleManager = FindFirstObjectByType<ObstacleManager>();
            _timer = FindAnyObjectByType<Timer>();
        }

        private void OnEnable()
        {
            _timer.OnWaveEnded += ResetCurrentCount;
        }

        private void OnDisable()
        {
            _timer.OnWaveEnded -= ResetCurrentCount;
        }

        /// <summary>
        /// Resets the count of placed obstacles when a wave ends.
        /// </summary>
        private void ResetCurrentCount()
        {
            for (var index = 0; index < obstacles.Count; index++)
            {
                var obstacle = obstacles[index];
                obstacle.canPlace = true;
                obstacle.currentCount = 0;
            }
        }

        /// <summary>
        /// Selects an obstacle for placement and initializes its preview.
        /// </summary>
        /// <param name="index">Index of the obstacle in the list.</param>
        public void SelectObstacle(int index)
        {
            if (index < 0 || index >= obstacles.Count) return;
            _selectedObstacleIndex = index;
            if (_previewObject != null)
            {
                Destroy(_previewObject);
            }
            _previewObject = Instantiate(obstacles[index].prefab);
            _previewObject.GetComponent<Collider>().enabled = false;
        }

        private void Update()
        {
            if (_selectedObstacleIndex == -1) return;
            UpdatePreviewPosition();

            if (Input.GetMouseButtonDown(0))
            {
                PlaceObstacle();
            }

            if (Input.GetMouseButtonDown(1))
            {
                RotateObstacle();
            }
        }

        /// <summary>
        /// Updates the position of the obstacle preview based on mouse position.
        /// </summary>
        private void UpdatePreviewPosition()
        {
            if (_selectedObstacleIndex == -1 || _previewObject == null) return;
            var obstacleData = obstacles[_selectedObstacleIndex];
            if (!obstacleData.canPlace)
            {
                _previewObject.SetActive(false);
                return;
            }
            var ray = cam.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out var hit))
            {
                _previewObject.SetActive(false);
                return;
            }
            var isValidPlacement = ((1 << hit.collider.gameObject.layer) & placementLayer) != 0;
            if (!isValidPlacement)
            {
                _previewObject.SetActive(false);
                return;
            }
            _definitivePos = new Vector3(hit.point.x, 0, hit.point.z);
            _previewObject.transform.position = _definitivePos;
            _previewObject.SetActive(true);
        }

        /// <summary>
        /// Rotates the obstacle preview by 90 degrees.
        /// </summary>
        private void RotateObstacle()
        {
            _rotationIndex = (_rotationIndex + 1) % 2;
            var newYRotation = _rotationIndex * 90f; 
            _actualRotation = Quaternion.Euler(0, newYRotation, 0);
            _previewObject.transform.rotation = _actualRotation;
        }

        /// <summary>
        /// Places the selected obstacle at the current preview position.
        /// </summary>
        private void PlaceObstacle()
        {
            if (_selectedObstacleIndex == -1 || _previewObject == null || !_previewObject.activeSelf) return;
            var obstacleData = obstacles[_selectedObstacleIndex];
            if (obstacleData.currentCount >= obstacleData.maxCount) return;

            var newObstacle = Instantiate(obstacleData.prefab, _definitivePos, _actualRotation);
            _obstacleManager.RegisterObstacle(newObstacle);

            obstacleData.currentCount++;
            if (obstacleData.currentCount >= obstacleData.maxCount)
            {
                obstacleData.canPlace = false;
                _previewObject.SetActive(false);
            }
            Destroy(_previewObject);
            _previewObject = null;
            _actualRotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
