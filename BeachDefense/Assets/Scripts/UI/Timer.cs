using System.Globalization;
using TMPro;
using UnityEngine;

namespace UI
{
    public class Timer : MonoBehaviour
    {
        public TextMeshProUGUI timerText;
        public float timeRemaining; 
        public bool isRunning;
        private bool _waveEndedInvoked = false;

        public delegate void WaveEnded(); 
        public event WaveEnded OnWaveEnded;  // Event triggered when a wave ends

        /// <summary>
        /// Starts game timer.
        /// </summary>
        public void StartTimer()
        {
            isRunning = true;
            _waveEndedInvoked = false;
        }

        private void Update()
        {
            if (isRunning && timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;

                if (timeRemaining < 0) 
                    timeRemaining = 0;

                timerText.text = Mathf.Ceil(timeRemaining).ToString(CultureInfo.InvariantCulture);
            }

            if (timeRemaining == 0 && !_waveEndedInvoked)
            {
                isRunning = false;
                Survived();
            }
        }
        /// <summary>
        /// Invokes an event when the timer reaches 0.
        /// </summary>
        private void Survived()
        {
            if (_waveEndedInvoked) return;
            _waveEndedInvoked = true;
            Debug.Log("Survived()");
            OnWaveEnded?.Invoke();
        }
        /// <summary>
        /// Changes the timer to the new time.
        /// </summary>
        /// <param name="newTime"></param>
        public void ResetTimer(float newTime)
        {
            timeRemaining = newTime;
            _waveEndedInvoked = false;
        }
    }
}