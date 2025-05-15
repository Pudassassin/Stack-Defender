using System;
using System.Collections.Generic;
using UnityEngine;

namespace StackDefender.Weapon
{
    public class ProjectileMover : MonoBehaviour
    {
        // simple projectile
        public Vector3 velocity = Vector3.zero;
        public Vector3 acceleration = Vector3.zero;
        public float projectileSize = 0.15f;

        // store and edit enemy to be hit based on whatever the projectile hit, usually the only first enemy hit
        public AttackDataObject attackData;
        
        Vector3 prevPos;
        List<GameObject> hitObjects = new List<GameObject>();

        private void Start()
        {
            transform.right = new Vector3(velocity.x, velocity.y, 0.0f);
        }

        private void Update()
        {
            // update position and velocity
            prevPos = transform.position;
            velocity += acceleration * Time.deltaTime;
            transform.position += velocity * Time.deltaTime;

            transform.right = new Vector3(velocity.x, velocity.y, 0.0f);

            // raycast and check collision
            if (ProjectileRaycast())
            {
                // check to only hit the intended target, then other enemy

                Destroy(gameObject);
            }

            // check ground (temp)
            if (transform.position.y <= 0.0f)
            {
                // might turn into some neat vfx later...

                Destroy(gameObject);
            }
        }

        private bool ProjectileRaycast()
        {
            // ideally cast a capsule collider from prev pos to current pos 
            Collider[] colliders = Physics.OverlapCapsule(prevPos, transform.position, projectileSize);
            if (colliders.Length > 0)
            {
                foreach (var item in colliders)
                {
                    if (item.gameObject.tag == "Enemy")
                    {
                        hitObjects.Add(item.gameObject);
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;

            Gizmos.DrawWireSphere(transform.position, projectileSize);
        }
    }
}
