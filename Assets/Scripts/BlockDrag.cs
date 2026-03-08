using UnityEngine;
using System.Collections.Generic; 

public class BlockDrag : MonoBehaviour
{
    private Vector3 startPosition;
    private Vector3 offset;

    private bool isPlaced = false; 

    void OnMouseDown()
    {
        if (isPlaced) return;

        if (GridManager.isGameOver) return;

        startPosition = transform.position;

        transform.localScale = Vector3.one; 

        offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    void OnMouseDrag()
    {
        if (isPlaced) return;

        if (GridManager.isGameOver) return;

        Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
        newPos.z = 0; 
        transform.position = newPos;
    }

    void OnMouseUp()
    {
        if (isPlaced) return;

        if (GridManager.isGameOver) return;

        float snapX = Mathf.Round(transform.position.x);
        float snapY = Mathf.Round(transform.position.y);
        Vector3 targetPos = new Vector3(snapX, snapY, 0f);

        transform.position = targetPos;

        bool canPlace = true;

        List<Vector2Int> tempOccupiedCells = new List<Vector2Int>();

        foreach (Transform child in transform)
        {
            int x = Mathf.RoundToInt(child.position.x);
            int y = Mathf.RoundToInt(child.position.y);

            if (GridManager.Instance.IsValidPosition(x, y) == false)
            {
                canPlace = false;
                break;
            }
            
            tempOccupiedCells.Add(new Vector2Int(x, y));
        }


        if (canPlace == true)
        {
            foreach (Transform child in transform)
            {
                int x = Mathf.RoundToInt(child.position.x);
                int y = Mathf.RoundToInt(child.position.y);
                GridManager.Instance.gridArray[x, y] = child; 
            }

            isPlaced = true; 
            BlockSpawner.Instance.BlockPlaced(); 

            GridManager.Instance.CheckLines();
        }
        else
        {
            transform.position = startPosition;
            transform.localScale = new Vector3(0.6f, 0.6f, 1f); 
        }
    }
}