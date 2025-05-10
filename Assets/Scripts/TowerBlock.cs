using UnityEngine;

namespace StackDefender.Block
{
    public class TowerBlock : MonoBehaviour
    {
        public static float BlockGravity = 1.0f;

        [HideInInspector]
        public Vector2Int gridPos = new Vector2Int(-1, -1);

        [HideInInspector]
        public GameObject stackAncherObject = null;

        float dropVelocity = 0.0f;
        bool isFalling = false;
        Vector3 landingPos;

        public void Update()
        {
            if (gridPos.x < 0 || gridPos.y < 0 || stackAncherObject == null) return;

            if (isFalling)
            {
                dropVelocity += BlockGravity * Time.deltaTime;
                Vector3 newPos = transform.position + new Vector3(0.0f, dropVelocity * Time.deltaTime, 0.0f);

                if (newPos.y < landingPos.y)
                {
                    newPos.y = landingPos.y;
                    isFalling = false;
                    // trigger landing effects
                }

                transform.position = newPos;
            }
        }

        public void TriggerFall()
        {
            if (gridPos.x < 0 || gridPos.y < 0 || stackAncherObject == null) return;

            TowerStackController stackController = stackAncherObject.GetComponent<TowerStackController>();
            Vector3 targetPos = stackController.GridPosToWorldspace(gridPos.x, gridPos.y);

            landingPos = targetPos;
            landingPos.y = transform.position.y;
            transform.position = landingPos;
            isFalling = true;
        }
    }
}
