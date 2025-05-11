using StackDefender.Block;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StackDefender.Block
{
    public class TowerStackController : MonoBehaviour
    {
        public bool DebugMode = true;

        public float BlockSize = 1.0f;
        // public static int StartingGridColumn = 9;
        // public static int StartingGirdRow = 7;

        // [HideInInspector]
        public int gridColumn = 9;
        // [HideInInspector]
        public int gridRow = 7;

        int hoverColumn = 0;

        // [left >>> right] [bottom >>> top]
        // List<List<GameObject>> towerColumns;
        GameObject[,] towerGrid;

        // [HideInInspector]
        public GameObject objectToPlace;

        public GameObject topCursorObject;
        public GameObject topCursorInvalidObject;
        public GameObject placeCursorObject;

        PlayerInput playerInput;
        InputAction PI_selectTarget, PI_moveCursor;

        bool targetHold;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            PI_selectTarget = InputSystem.actions.FindAction("Select Target");
            PI_moveCursor = InputSystem.actions.FindAction("Position Cursor");

            towerGrid = new GameObject[gridColumn, gridRow];
            for (int x = 0; x < gridColumn; x++)
            {
                for (int y = 0; y < gridRow; y++)
                {
                    towerGrid[x, y] = null;
                }
            }

            // towerColumns = new List<List<GameObject>>();
            // for (int column = 0; column < gridColumn; column++)
            // {
            //     towerColumns.Add(new List<GameObject>());
            // }

            // gridColumn = StartingGridColumn;
            // gridRow = StartingGirdRow;
        }

        // Update is called once per frame
        void Update()
        {
            MouseHoverOnColumn();
            UpdateCursors();

            MousePlaceBlock();
        }

        public void MouseHoverOnColumn()
        {
            Camera camera = Camera.main;
            Vector2 mousePos = PI_moveCursor.ReadValue<Vector2>();
            Vector3 pointerPos = camera.ScreenToWorldPoint(mousePos);

            if (pointerPos.x < transform.position.x)
            {
                hoverColumn = 0;
            }
            else if (pointerPos.x > transform.position.x + BlockSize * gridColumn)
            {
                hoverColumn = gridColumn - 1;
            }
            else
            {
                hoverColumn = Mathf.FloorToInt((pointerPos - transform.position).x / BlockSize);
            }
        }

        public void MousePlaceBlock()
        {
            if (PI_selectTarget.IsPressed())
            {
                if (!targetHold)
                {
                    // trigger once per click
                    //if (towerColumns[hoverColumn].Count < gridRow)
                    if (gridRow - GetStackHeight(hoverColumn) > 0)
                    {
                        SpawnAndDropBlock();
                    }

                    targetHold = true;
                }
            }
            else
            {
                targetHold = false;
            }
        }

        public void UpdateCursors()
        {
            // top cursor
            Vector3 newTopPos = transform.position;
            GameObject currentCursor;
            newTopPos.y += BlockSize * (gridRow + 1.0f);
            newTopPos.x += BlockSize * (hoverColumn + 0.5f);
            placeCursorObject.transform.position = GridPosToWorldspace(hoverColumn, GetStackHeight(hoverColumn));

            if (GetStackHeight(hoverColumn) >= gridRow)
            {
                // stack is full
                topCursorObject.SetActive(false);
                topCursorInvalidObject.SetActive(true);
                placeCursorObject.SetActive(false);

                currentCursor = topCursorInvalidObject;
            }
            else
            {
                // stack has room
                topCursorObject.SetActive(true);
                topCursorInvalidObject.SetActive(false);
                placeCursorObject.SetActive(true);

                currentCursor = topCursorObject;
            }

            currentCursor.transform.position = newTopPos;
        }

        public GameObject SpawnAndDropBlock()
        {
            GameObject blockObject = GameObject.Instantiate(objectToPlace);
            blockObject.transform.position = topCursorObject.transform.position;

            TowerBlock towerBlock = blockObject.GetComponent<TowerBlock>();
            towerBlock.stackAncherObject = this.gameObject;
            // towerBlock.gridPos = new Vector2Int(hoverColumn, towerColumns[hoverColumn].Count);
            Vector2Int gridPos = new Vector2Int(hoverColumn, GetStackHeight(hoverColumn));
            towerBlock.gridPos = gridPos;
            towerBlock.TriggerFall();

            // towerColumns[hoverColumn].Add(blockObject);
            towerGrid[gridPos.x, gridPos.y] = blockObject;

            return blockObject;
        }

        public Vector3 GridPosToWorldspace(int column, int row, bool center = true)
        {
            Vector3 result = transform.position;
            result.x += column * BlockSize;
            result.y += row * BlockSize;

            if (center)
            {
                result += new Vector3(BlockSize * 0.5f, BlockSize * 0.5f, 0.0f);
            }

            return result;
        }

        public int GetStackHeight(int column)
        {
            int result = 0;

            for (; result < gridRow; result++)
            {
                if (result >= gridRow) break;
                else if (towerGrid[hoverColumn, result] == null) break;
            }

            return result;
        }

        private void OnDrawGizmos()
        {
            if (!DebugMode) return;

            Gizmos.color = Color.yellow;

            float gridHeight = BlockSize * gridRow;
            float gridWidth = BlockSize * gridColumn;

            Vector3 topLeft = transform.position + new Vector3(0.0f, gridHeight, 0.0f);
            Vector3 topRight = transform.position + new Vector3(gridWidth, gridHeight, 0.0f);
            Vector3 bottomRight = transform.position + new Vector3(gridWidth, 0.0f, 0.0f);

            for (int column = 0; column <= gridColumn; column++)
            {
                Vector3 bottom = transform.position;
                bottom.x += BlockSize * column;

                Vector3 top = bottom + new Vector3(0.0f, gridHeight, 0.0f);
                Gizmos.DrawLine(bottom, top);
            }

            for (int row = 0; row <= gridRow; row++)
            {
                Vector3 left = transform.position;
                left.y += BlockSize * row;

                Vector3 right = left + new Vector3(gridWidth, 0.0f, 0.0f);
                Gizmos.DrawLine(left, right);
            }
        }
    }
}