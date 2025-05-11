using UnityEditor;
using UnityEngine;

namespace StackDefender.Block
{
    public class TowerBlock : MonoBehaviour
    {
        public float BlockGravity = 10.0f;

        [HideInInspector]
        public Vector2Int gridPos = new Vector2Int(-1, -1);

        [HideInInspector]
        public GameObject stackAncherObject = null;

        float dropVelocity = 0.0f;
        bool isFalling = false;
        Vector3 landingPos;

        public GameObject weaponObject;

        public void FixedUpdate()
        {
            if (gridPos.x < 0 || gridPos.y < 0 || stackAncherObject == null) return;

            if (isFalling)
            {
                dropVelocity += BlockGravity * Time.deltaTime;
                Vector3 newPos = transform.position + new Vector3(0.0f, -dropVelocity * Time.deltaTime, 0.0f);

                if (newPos.y < landingPos.y)
                {
                    newPos.y = landingPos.y;
                    isFalling = false;

                    // trigger landing effects
                    weaponObject.SetActive(true);

                }

                transform.position = newPos;
            }
        }

        public void TriggerFall()
        {
            if (gridPos.x < 0 || gridPos.y < 0 || stackAncherObject == null) return;

            // trigger falling effects
            weaponObject.SetActive(false);

            TowerStackController stackController = stackAncherObject.GetComponent<TowerStackController>();
            Vector3 targetPos = stackController.GridPosToWorldspace(gridPos.x, gridPos.y);

            landingPos = targetPos;
            targetPos.y = transform.position.y;
            transform.position = targetPos;
            isFalling = true;
        }
    }
}
