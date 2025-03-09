using System;
using System.Collections.Generic;
using UI;
using UnityEngine;

namespace Managers
{
   /// <summary>
   /// Manages the overall game logic, including waves, enemies, and win/loss conditions.
   /// </summary>
   public class GameManager : MonoBehaviour
   {
      [Header("Wave Settings")]
      public List<int> timeOfWaves; 
      public int waveCount; 
      
      [Header("Other Settings")]
      public List<GameObject> allEnemies;
      public int towerCount;
      [SerializeField] private AudioSource winSfx;
    
         
      public delegate void StartWaves(int waveCount);
      public StartWaves OnStartWaves; // Event triggered when a wave starts

      public delegate void Winner();
      public Winner OnWinner; // Event triggered when the player wins

      public delegate void Loose();
      public Loose OnLoose; // Event triggered when the player loses
      
      private List<GameObject> _enemiesCopy; 
      [SerializeField] private PlacementManager placementManager; 
      private Timer _timer;

      private void Awake()
      {
         _timer = FindAnyObjectByType<Timer>();
      }

      private void Start()
      {
         _timer = FindAnyObjectByType<Timer>(); // Finds the Timer instance in the scene
         _enemiesCopy = new List<GameObject>(); 
      }

      private void OnEnable()
      {
         _timer.OnWaveEnded += WaveSurvived; // Subscribes to the wave-ended event
      }

      private void OnDisable()
      {
         _timer.OnWaveEnded -= WaveSurvived; // Unsubscribes from the wave-ended event
      }

      /// <summary>
      /// Initiates a new wave, clearing any remaining enemies and adjusting obstacle limits.
      /// </summary>
      public void SendWave()
      {
         if (_enemiesCopy.Count != 0)
         {
            _enemiesCopy.Clear();
         }
         OnStartWaves?.Invoke(waveCount);
         
         waveCount++;
         switch (waveCount)
         {
            case 1 : 
               placementManager.obstacles[0].maxCount = 3;
               placementManager.obstacles[1].maxCount = 5;
               placementManager.obstacles[2].maxCount = 5;
               break;
             case 2:
                placementManager.obstacles[0].maxCount = 5;
                placementManager.obstacles[1].maxCount = 6;
                placementManager.obstacles[2].maxCount = 8;
                break;
             case 3:
                placementManager.obstacles[0].maxCount = 2;
                placementManager.obstacles[1].maxCount = 1;
                placementManager.obstacles[2].maxCount = 10;
                break;
         }
      }

      /// <summary>
      /// Adds an enemy to the list of active enemies.
      /// </summary>
      public void AddEnemy(GameObject enemy)
      {
         allEnemies.Add(enemy);
      }

      /// <summary>
      /// Removes a killed enemy from the list of active enemies.
      /// </summary>
      public void EnemyKilled(GameObject enemy)
      {
         allEnemies.Remove(enemy);
      }

      /// <summary>
      /// Called when a tower is destroyed. Triggers game over if all towers are lost.
      /// </summary>
      public void DeadTower()
      {
         towerCount--;
         if (towerCount == 0)
         {
            GameOver();
         }
      }

      /// <summary>
      /// Handles wave survival, disabling all remaining enemies and progressing waves.
      /// </summary>
      private void WaveSurvived()
      {
         _enemiesCopy = new List<GameObject>(allEnemies);
         foreach (var enemy in _enemiesCopy)
         {
            enemy.SetActive(false);
         }
         allEnemies.Clear();

         if (waveCount == 3)
         {
            Win();
         }
         winSfx.Play();
         if (timeOfWaves.Count <= waveCount) return;
         _timer.ResetTimer(timeOfWaves[waveCount]);
      }

      /// <summary>
      /// Triggers the win event when all waves are completed.
      /// </summary>
      private void Win()
      {
         OnWinner?.Invoke();
      }

      /// <summary>
      /// Triggers the game over event when all towers are destroyed.
      /// </summary>
      private void GameOver()
      {
         Debug.Log("<color=red>Game Over</color>");
         OnLoose?.Invoke();
      }
   }
}