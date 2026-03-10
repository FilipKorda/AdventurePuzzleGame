using System.Collections.Generic;
using UnityEngine;

public class BlockMovementTest : MonoBehaviour
{
    public float moveDistance = 0.8f;
    public float moveSpeed = 4f;

    public BlockType blockType;
    public BlockDirection direction;
    public Vector2 gridPosition;

    public GridOccupancy grid;

    private Vector3 targetPosition;

    public bool canControl = false;

    void Start()
    {
        targetPosition = transform.localPosition;

        if (grid == null)
        {
            BlockSpawner spawner = FindObjectOfType<BlockSpawner>();
            if (spawner != null)
            {
                grid = spawner.Grid;
            }
            else
            {
                Debug.LogError("Nie znaleziono BlockSpawner w scenie!");
            }
        }
    }

    void Update()
    {
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPosition, moveSpeed * Time.deltaTime);

        HandleInput();
    }

    void HandleInput()
    {
        // blokujemy ruch jeśli blok jeszcze nie dojechał do targetPosition
        if (Vector3.Distance(transform.localPosition, targetPosition) > 0.01f) return;

        Vector3 moveDelta = Vector3.zero;
        Vector2 gridDelta = Vector2.zero;

        if(canControl)
        {
            // ograniczamy ruch do osi poziomej lub pionowej w zależności od kierunku bloku
            if (direction == BlockDirection.Z) // poziomo → tylko lewo/prawo
            {
                if (Input.GetKeyDown(KeyCode.D))
                {
                    moveDelta = new Vector3(moveDistance, 0, 0);
                    gridDelta = new Vector2(1, 0);
                }
                else if (Input.GetKeyDown(KeyCode.A))
                {
                    moveDelta = new Vector3(-moveDistance, 0, 0);
                    gridDelta = new Vector2(-1, 0);
                }
            }
            else if (direction == BlockDirection.X) // pionowo → tylko góra/dół
            {
                if (Input.GetKeyDown(KeyCode.W))
                {
                    moveDelta = new Vector3(0, 0, moveDistance);
                    gridDelta = new Vector2(0, 1);
                }
                else if (Input.GetKeyDown(KeyCode.S))
                {
                    moveDelta = new Vector3(0, 0, -moveDistance);
                    gridDelta = new Vector2(0, -1);
                }
            }
        }
      

        if (moveDelta == Vector3.zero) return;

        if (moveDelta == Vector3.zero) return;
        // blokujemy nowe kratki
        Vector2 newGridPos = gridPosition + gridDelta;
        List<Vector2> newCells = BlockGridUtility.GetOccupiedCells(newGridPos, blockType, direction);

        // odblokowujemy stare kratki
        List<Vector2> oldCells = BlockGridUtility.GetOccupiedCells(gridPosition, blockType, direction);

        foreach (var cell in newCells)
        {
            if (grid.IsOccupied(cell) && !oldCells.Contains(cell))
                return;
        }

        foreach (var cell in oldCells)
            grid.OccupiedCells.Remove(cell);


        foreach (var cell in newCells)
            grid.SetOccupied(cell);

        // aktualizujemy pozycję w gridzie i targetPosition
        gridPosition = newGridPos;
        targetPosition += moveDelta;
    }
}