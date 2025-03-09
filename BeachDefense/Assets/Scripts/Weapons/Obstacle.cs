using UnityEngine;

namespace Weapons
{
    [CreateAssetMenu(fileName = "Obstacle", menuName = "Scriptable Objects/Obstacle")]
    public class Obstacle : ScriptableObject
    {
        public float health;
        public int damage;
        public new string name;
        public string description;
    }
}
