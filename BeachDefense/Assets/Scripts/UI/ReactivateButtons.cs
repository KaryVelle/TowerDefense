using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// Controls the activation and deactivation of a button based on game wave events.
    /// </summary>
    public class ReactivateButtons : MonoBehaviour
    {
        [SerializeField] private GameManager manager;
        [SerializeField] private Timer timer;
        private Button _btn;

        private void Awake()
        {
            _btn = GetComponent<Button>();
        }

        /// <summary>
        /// Subscribes to game events when enabled.
        /// </summary>
        protected void OnEnable()
        {
            manager.OnStartWaves += GoButtonDeactivate;
            timer.OnWaveEnded += GoButtonActive;
        }

        /// <summary>
        /// Unsubscribes from game events when disabled.
        /// </summary>
        protected void OnDisable()
        {
            manager.OnStartWaves -= GoButtonDeactivate;
            timer.OnWaveEnded -= GoButtonActive;
        }

        /// <summary>
        /// Activates the button when a wave ends.
        /// </summary>
        private void GoButtonActive()
        {
            _btn.interactable = true;
        }

        /// <summary>
        /// Deactivates the button when a new wave starts.
        /// </summary>
        /// <param name="a">Integer parameter required by the event delegate.</param>
        public void GoButtonDeactivate(int a)
        {
            _btn.interactable = false;
        }
    }
}