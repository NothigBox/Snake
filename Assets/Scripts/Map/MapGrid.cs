using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGrid : MonoBehaviour
{
    [SerializeField] int width = 1;
    [SerializeField] int height = 1;
    [SerializeField] float cellSize = 1f;

    private void OnDrawGizmos()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                //  Get the center position of each cell in the grid
                float positionX = transform.position.x + (cellSize * ((1 / 2f) + x));
                float positionY = transform.position.y + (cellSize * ((1 / 2f) + y));

                Vector2 position = new Vector2(positionX, positionY);

                //  Draw a cell with its center at grid positions, with the desired size
                Gizmos.DrawWireCube(position, Vector3.one * cellSize);
            }
        }
    }

    public Vector2 GetRandomAvailablePosition(params Transform[] unavailablePositions)
    {
        Vector2 result = Vector2.zero;

        List<Vector2> UnavailableCells = new List<Vector2>();

        for (int i = 0; i < unavailablePositions.Length; i++)
        {
            Vector2 cell = FromPositionToCell(unavailablePositions[i].position);
            UnavailableCells.Add(cell);
        }

        Debug.Log("Unavaliable Cells");

        for (int i = 0; i < UnavailableCells.Count; i++)
        {
            Debug.Log($"Cell: {UnavailableCells[i]}");
        }

        Debug.Log("END Unavaliable Cells");

        List<Vector2> fullGrid = GetFullGridCells();

        for (int i = 0; i < UnavailableCells.Count; i++)
        {
            //  Convert cell coordenates into int numbers
            int cellX = Mathf.RoundToInt(UnavailableCells[i].x);
            int cellY = Mathf.RoundToInt(UnavailableCells[i].y);

            //  The UnavailableCells list must be sorted as the fullGrid (a series of raws, from minor to major)
            SortUnavailableCells(ref UnavailableCells);

            //  Get the cell index for the cells that need to be removed
            int cellIndex = cellY * width + cellX;
            int finalIndex = cellIndex - i;

            Debug.Log($"Cell removed: {UnavailableCells[i]} => {fullGrid[finalIndex]} | Index: {cellIndex} y {finalIndex}");
            fullGrid.RemoveAt(finalIndex);
        }

        Vector2 randomCell = fullGrid[UnityEngine.Random.Range(0, fullGrid.Count)];
        result = FromCellToPosition(randomCell);

        return result;
    }

    public Vector2 FromPositionToCell(Vector2 position)
    {
        Vector2 cell = default;

        float cellX = ((position.x - transform.position.x) / cellSize) - (1 / 2f);
        float cellY = ((position.y - transform.position.y) / cellSize) - (1 / 2f);

        cell = new Vector2(cellX, cellY);

        return cell;
    }

    public Vector2 FromCellToPosition(Vector2 cell)
    {
        Vector2 position = default;

        //  Get the center position of each cell in the grid
        float positionX = transform.position.x + (cellSize * ((1 / 2f) + cell.x));
        float positionY = transform.position.y + (cellSize * ((1 / 2f) + cell.y));

        position = new Vector2(positionX, positionY);

        return position;
    }

    public List<Vector2> GetFullGridCells()
    {
        List<Vector2> cells = new List<Vector2>();

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                cells.Add(new Vector2(x, y));
            }
        }

        for (int i = 0; i < cells.Count; i++)
        {
            Debug.Log(cells[i]);
        }

        return cells;
    }

    void SortUnavailableCells(ref List<Vector2> unavailableCells)
    {

    }
}
