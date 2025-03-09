using Enemy;
using UnityEngine;

namespace Weapons
{
    /// <summary>
    /// Represents an obstacle with spikes that deals periodic damage to enemies within its trigger zone.
    /// </summary>
    public class ObstacleSpikes : MonoBehaviour
    {
        [SerializeField] private Obstacle obstacle; 
        [SerializeField] private float damageInterval = 1; 
        [SerializeField] private EnemyActions enemy;

        /// <summary>
        /// Starts dealing damage to an enemy when it enters the trap's trigger zone.
        /// </summary>
        /// <param name="other">The collider of the object that entered the trigger.</param>
        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.layer != 7) return;
            enemy = other.gameObject.GetComponent<EnemyActions>();
            if (enemy == null) return;
            
            if (!IsInvoking(nameof(DealDamage))) 
            {
                InvokeRepeating(nameof(DealDamage), 0f, damageInterval);
            }
        }

        /// <summary>
        /// Stops dealing damage when the enemy exits the trap's trigger zone.
        /// </summary>
        /// <param name="other">The collider of the object that exited the trigger.</param>
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer != 7) return;
            CancelInvoke(nameof(DealDamage)); // Stop the damage coroutine.
        }

        /// <summary>
        /// Inflicts damage on the enemy currently inside the trap.
        /// </summary>
        private void DealDamage()
        {
            if (enemy == null) return;
            enemy.ReceiveDamage(obstacle.damage); // Apply damage to the enemy.
            Debug.Log($"<color=green>Trap inflicts {obstacle.damage} damage to {enemy.name}</color>");
        }
    }
}