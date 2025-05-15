using System.Collections.Generic;
using UnityEngine;

namespace StackDefender.Weapon
{
    public class AttackDataObject : MonoBehaviour
    {
        public List<GameObject> intendedTargets, suitableTargets;
        public float damage;


        void Start()
        {
            // initial setup

        }

        // ... 
        void Update()
        {

        }

        // Collider event listen for attack to land

        // Attack trigger
        public void ResolveAttack()
        {
            // inflict damage on the target(s) listed when called

        }
    }
}
