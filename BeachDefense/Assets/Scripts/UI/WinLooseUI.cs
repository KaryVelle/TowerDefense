using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
   public class WinLooseUI : MonoBehaviour
   {
      [SerializeField] private Canvas canvas;
      [SerializeField] private TextMeshProUGUI tmp;
      [SerializeField] private GameManager manager;
      
      /// <summary>
      /// Subscribes to game events when enabled.
      /// </summary>
      private void OnEnable()
      {
         manager.OnWinner += OnWin;
         manager.OnLoose += OnLoose;
      }
      
      /// <summary>
      /// Unsubscribes from game events when disabled.
      /// </summary>
      private void OnDisable()
      {
         manager.OnWinner -= OnWin;
         manager.OnLoose -= OnLoose;
      }
      
      /// <summary>
      /// Enables and actualizes the ending canvas.
      /// </summary>
      private void OnWin()
      {
         Time.timeScale = 0;
         canvas.enabled = true;
         tmp.text = "Winner!";
      }
      
      /// <summary>
      /// Enables and actualizes the ending canvas.
      /// </summary>
      private void OnLoose()
      {
         Time.timeScale = 0;
         canvas.enabled = true;
         tmp.text = "Game Over";
      }
      
      /// <summary>
      /// Load the GameScene.
      /// </summary>
      public void Restart()
      {
         SceneManager.LoadScene(0);
         Time.timeScale = 1;
      }
   }
}
