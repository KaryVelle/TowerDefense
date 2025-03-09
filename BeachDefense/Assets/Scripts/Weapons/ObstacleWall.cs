using System.Collections;
using Enemy;
using UnityEngine;

namespace Weapons
{
    /// <summary>
    /// Represents a destructible wall obstacle that takes damage from enemy collisions.
    /// </summary>
    public class ObstacleWall : MonoBehaviour
    {
        [SerializeField] private Obstacle obstacle; 
        private bool _isReceivingDamage;
        public float myHealth;

        private void Awake()
        {
            myHealth = obstacle.health; // Initialize wall health.
        }

        /// <summary>
        /// Starts receiving damage when an enemy collides with the wall.
        /// </summary>
        /// <param name="other">The collision object.</param>
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.layer != 7) return;
            _isReceivingDamage = true;
            StartCoroutine(ReceiveDamage(other.gameObject.GetComponent<EnemyActions>()));
        }

        /// <summary>
        /// Stops receiving damage when the enemy exits collision.
        /// </summary>
        /// <param name="other">The collision object.</param>
        private void OnCollisionExit(Collision other)
        {
            if (other.gameObject.layer == 7)
            {
                _isReceivingDamage = false;
            }
        }

        /// <summary>
        /// Coroutine that continuously reduces the wall's health while under attack.
        /// </summary>
        /// <param name="enemy">The enemy causing the damage.</param>
        private IEnumerator ReceiveDamage(EnemyActions enemy)
        { 
            while (_isReceivingDamage && myHealth > 0)
            {
                myHealth -= enemy.damage; 

                if (myHealth <= 0)
                {
                    KillWall();
                    yield break;
                }

                yield return new WaitForSeconds(1f);
            }

            if (!(myHealth <= 0)) KillWall(); 
        }

        /// <summary>
        /// Disables the wall when destroyed.
        /// </summary>
        private void KillWall()
        {
            gameObject.SetActive(false);
        }
    }
}