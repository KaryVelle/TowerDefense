using System.Collections;
using UnityEngine;

namespace Enemy
{
    /// <summary>
    /// Handles specific enemy actions such as movement and receiving damage.
    /// </summary>
    public class EnemyActions : Enemy
    {
        /// <summary>
        /// Called when the enemy is enabled. Initializes movement and registers with the GameManager.
        /// </summary>
        public void OnEnable()
        {
            StartCoroutine(WaitToSetDestination());
            manager.AddEnemy(gameObject);
        }

        /// <summary>
        /// Called when the enemy is disabled. Notifies the GameManager that the enemy was killed.
        /// </summary>
        private void OnDisable()
        {
            manager.EnemyKilled(gameObject);
        }

        /// <summary>
        /// Reduces health when the enemy takes damage. Calls Die() if health reaches zero.
        /// </summary>
        /// <param name="damageReceived">Amount of damage received</param>
        public override void ReceiveDamage(float damageReceived)
        {
            Debug.Log("Receiving Damage " + damageReceived);
            health -= damageReceived;
            if (!(health <= 0f)) return;
            Die();
        }

        /// <summary>
        /// Handles enemy death by disabling the GameObject.
        /// </summary>
        public override void Die()
        {
            gameObject.SetActive(false);
            Debug.Log("Dead");
        }

        /// <summary>
        /// Waits for a short duration before setting the enemy's destination randomly.
        /// </summary>
        private IEnumerator WaitToSetDestination()
        {
            yield return new WaitForSeconds(1);
            var selectedTarget = Random.Range(0, destination.Count);
            agent.SetDestination(destination[selectedTarget]);
        }
    }
}