using System;
using Managers;
using UnityEngine;

namespace Towers
{
    /// <summary>
    /// Represents a tower in the game that can take damage and be destroyed.
    /// </summary>
    public class Tower : MonoBehaviour
    {
        public float health; 
        private GameManager _manager;

        private void Awake()
        {
            _manager = FindAnyObjectByType<GameManager>();
        }

        /// <summary>
        /// Handles the tower's destruction when health reaches zero.
        /// </summary>
        private void Die()
        {
            _manager.DeadTower();
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Handles damage reception when colliding with an enemy.
        /// </summary>
        /// <param name="other">The collider of the object that entered the trigger.</param>
        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer != 7) return;
            var receivedDamage = other.GetComponent<Enemy.Enemy>().damage;
            Debug.Log(receivedDamage);
            health -= receivedDamage;
            if (health <= 0)
            {
                Die();
            }
        }
    }
}