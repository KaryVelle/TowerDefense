using Managers;
using TMPro;
using UnityEngine;

namespace UI
{
    public class UpdateWavePanel : MonoBehaviour
    {
        private GameManager _manager;
        [SerializeField] private Timer timer;
        [SerializeField] private TextMeshProUGUI tmp;
        
        private void Awake()
        {
          _manager = FindFirstObjectByType<GameManager>();
          timer = FindAnyObjectByType<Timer>();
        }
        /// <summary>
        /// Subscribes to game events when enabled.
        /// </summary>
        private void OnEnable()
        {
            timer.OnWaveEnded += WaveSurvived;
        }
        
        /// <summary>
        /// Subscribes to game events when enabled.
        /// </summary>
        private void OnDisable()
        {
            timer.OnWaveEnded -= WaveSurvived;
        }
        
        /// <summary>
        /// Updates the wave count in the UI text.
        /// </summary>
        private void WaveSurvived()
        {
            var count = _manager.waveCount + 1;
            tmp.text = "WAVE " + count;
        }
    }
}
