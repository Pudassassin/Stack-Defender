using UnityEngine;
using UnityEngine.InputSystem;

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

    // [HideInInspector]
    public GameObject objectToPlace;
    public GameObject topCursorObject;

    PlayerInput playerInput;
    InputAction PI_selectTarget, PI_moveCursor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PI_selectTarget = InputSystem.actions.FindAction("Select Target");
        PI_moveCursor = InputSystem.actions.FindAction("Position Cursor");
        // gridColumn = StartingGridColumn;
        // gridRow = StartingGirdRow;
    }

    // Update is called once per frame
    void Update()
    {
        MouseHoverOnColumn();
        UpdateCursors();
    }

    public void MouseHoverOnColumn()
    {
        Camera camera = Camera.main;
        Vector3 pointerPos = camera.ScreenToWorldPoint(Input.mousePosition);

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
        if (playerInput.)
        {

        }
    }

    public void UpdateCursors()
    {
        // top cursor
        Vector3 newTopPos = transform.position;
        newTopPos.y += BlockSize * (gridRow + 1.0f);
        newTopPos.x += BlockSize * (hoverColumn + 0.5f);
        topCursorObject.transform.position = newTopPos;
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
