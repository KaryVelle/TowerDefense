using System.Collections.Generic;
using Managers;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    /// <summary>
    /// Abstract base class for enemy behavior.
    /// Handles common attributes such as health, speed, and damage.
    /// </summary>
    public abstract class Enemy : MonoBehaviour
    {
        public GameManager manager; 
        public NavMeshAgent agent; 
        public List<Vector3> destination; // Possible destinations for the enemy
        public float health; 
        public float speed;
        public float damage; 

        /// <summary>
        /// Method to be implemented for handling damage received by the enemy.
        /// </summary>
        /// <param name="damage">Amount of damage taken</param>
        public abstract void ReceiveDamage(float damage);

        /// <summary>
        /// Method to be implemented for handling enemy death.
        /// </summary>
        public abstract void Die();

        /// <summary>
        /// Initializes essential components when the enemy is created.
        /// </summary>
        public void Awake()
        {
            manager = FindAnyObjectByType<GameManager>();
            agent = GetComponent<NavMeshAgent>();
        }

        /// <summary>
        /// Sets up the enemy's speed and acceleration upon spawning.
        /// </summary>
        public void Start()
        {
            agent.speed = speed;
            agent.acceleration = speed * 0.8f;
        }
    }
}