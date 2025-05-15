using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace StackDefender.Weapon
{
    public class AttackHandler : MonoBehaviour
    {
        public enum Stats
        {
            windupTime,

            splashRadius = 10,
            splashDamageMul,

            projectileSpeed = 20,
            projectileAcc,
            projectileGravity,
            projectilePierce,
            projectileChain

        }

        public enum TargetPriority
        {
            front,
            back,
            mostHP,
            leastHP,
            fastest,
            slowest,
            strongest,
            weakest
        }

        [Serializable]
        public class WeaponExtraStat
        {
            public Stats stats;
            public float value;

            public WeaponExtraStat(Stats statsType, float value)
            {
                this.stats = statsType;
                this.value = value;
            }

            public static WeaponExtraStat QueryStats(List<WeaponExtraStat> statsList, Stats queryStats)
            {
                WeaponExtraStat result = null;
                foreach (var item in statsList)
                {
                    if (item.stats == queryStats)
                    {
                        result = item;
                        break;
                    }
                }

                return result;
            }
        }

        public GameObject attackPrefab;
        public float damage = 5.0f;
        public float attackRate = 2.0f;
        public float weaponRange = 3.5f;
        public TargetPriority targetPriority = TargetPriority.front;

        List<GameObject> targets;

        public float attackCooldown
        {
            get
            {
                return 1.0f / attackRate;
            }
            set
            {
                attackRate = 1.0f / value;
            }
        }

        public List<WeaponExtraStat> statsList = new List<WeaponExtraStat>();

        // internal vars
        float weaponTimer = 0.0f;


        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            weaponTimer += Time.deltaTime;

            if (weaponTimer > attackCooldown)
            {
                // enable targetting
                targets = AcquireTarget();
                if (targets.Count >= 1)
                {
                    // temp measure
                    Attack();
                    // Debug.Log(targetObjects[0]);
                    weaponTimer -= attackCooldown;
                }
                else
                {
                    weaponTimer = attackCooldown;
                }
            }
        }

        public List<GameObject> AcquireTarget(int targetCount = 1)
        {
            List<GameObject> resultList = new List<GameObject>();
            List<GameObject> tempList;

            // first enemy closest to castle
            {
                tempList = GameObject.FindGameObjectsWithTag("Enemy").ToList();

                // exclude enemy outside of weapon range
                for (int i = tempList.Count - 1; i >= 0; i--)
                {
                    float distanceFromTower = Mathf.Abs(tempList[i].transform.position.x - transform.position.x);
                    if (distanceFromTower > weaponRange)
                    {
                        tempList.RemoveAt(i);
                    }
                }

                // sort priority
                tempList.Sort(CompareFirstToCastle);

                if (tempList.Count > targetCount)
                {
                    tempList.RemoveRange(targetCount, tempList.Count - targetCount);
                }

                resultList = tempList;
            }

            return resultList;
        }

        public void Attack()
        {
            GameObject attackObj = Instantiate(attackPrefab);
            attackObj.transform.position = transform.position;

            AttackDataObject attackData = attackObj.GetComponent<AttackDataObject>();
            attackData.intendedTargets = targets;
            attackData.damage = damage;

            // temp -- typical straight shot projectile
            if (attackObj.GetComponent<ProjectileMover>())
            {
                ProjectileMover mover = attackObj.GetComponent<ProjectileMover>();
                Vector3 direction = (targets[0].transform.position - transform.position).normalized;
                mover.velocity = direction * ReadStats(Stats.projectileSpeed);
            }
        }

        private void OnEnable()
        {
            weaponTimer = 0.0f;
        }

        // comparation delegate
        public float ReadStats(Stats stats)
        {
            foreach (var item in statsList)
            {
                if (item.stats == stats)
                {
                    return item.value;
                }
            }

            Debug.LogWarning("Stats is missing: " + gameObject.name + " - " + stats.ToString());
            return 0.1f;
        }

        public static int CompareFirstToCastle(GameObject objectA, GameObject objectB)
        {
            if (objectA == null)
            {
                if (objectB == null)
                {
                    // Neither exists...
                    return 0;
                }
                else
                {
                    // B exists-- A deems farther
                    return 1;
                }
            }
            else
            {
                if (objectB == null)
                {
                    // A exists and deems closer
                    return -1;
                }
                else
                {
                    float aX = objectA.transform.position.x;
                    float bX = objectB.transform.position.x;

                    if (aX < bX)
                    {
                        // A is closer
                        return -1;
                    }
                    else if (aX > bX)
                    {
                        // A is farther
                        return 1;
                    }
                    else
                    {
                        // rare, both is at same distance
                        return 0;
                    }
                }
            }
        }
    }
}
