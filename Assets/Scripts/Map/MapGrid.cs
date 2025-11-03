using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class MapGrid : MonoBehaviour
{
    [SerializeField] int width = 1;
    [SerializeField] int height = 1;
    [SerializeField] float cellSize = 1f;

    [Header("Opciones de Escalado")]
    [SerializeField] bool fillScreen = false; // Si es true, llena la pantalla (puede cortar el mapa)
    [SerializeField] float padding = 0.1f; // Margen en unidades de mundo

    public Action<float, Vector2> OnCellSizeCalculated;

    /// <summary>
    /// The scale that objects must have in Unity
    /// </summary>
    public float CellSize => cellSize;

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
        //  Convert snake body's positions into cell coordinates
        Vector2 result = Vector2.zero;

        List<Vector2> unavailableCells = new List<Vector2>();

        for (int i = 0; i < unavailablePositions.Length; i++)
        {
            Vector2 cell = FromPositionToCell(unavailablePositions[i].position);
            unavailableCells.Add(cell);
        }

        /*
        Debug.Log("Unavaliable Cells");

        for (int i = 0; i < UnavailableCells.Count; i++)
        {
            Debug.Log($"Cell: {UnavailableCells[i]}");
        }

        Debug.Log("END Unavaliable Cells");
        */

        //  Take the UnavailableCells out from the fullGrid cell's list
        List<Vector2> fullGrid = GetFullGridCells();

        //  The unavailableCells list must be sorted as the fullGrid (a series of raws, from minor to major)
        SortUnavailableCells(ref unavailableCells);

        for (int i = 0; i < unavailableCells.Count; i++)
        {
            //  Convert cell coordenates into int numbers
            int cellX = Mathf.RoundToInt(unavailableCells[i].x);
            int cellY = Mathf.RoundToInt(unavailableCells[i].y);

            //  Get the cell index for the cells that need to be removed
            int cellIndex = cellY * width + cellX;
            int finalIndex = cellIndex - i;

            //Debug.Log($"Cell removed: {UnavailableCells[i]} => {fullGrid[finalIndex]} | Index: {cellIndex} y {finalIndex}");
            fullGrid.RemoveAt(finalIndex);
        }

        //  Return the position of a random cell from the fullGrid list, which doesn't contain any unavailable cell
        Vector2 randomCell = fullGrid[UnityEngine.Random.Range(0, fullGrid.Count)];
        result = FromCellToPosition(randomCell);

        return result;
    }

    public Vector2 FromPositionToCell(Vector2 position)
    {
        Vector2 cell = default;

        //  Get the cell coordinates correspondant to a given position in the world
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
    // Create a temporal list
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

        return cells;
    }

    void SortUnavailableCells(ref List<Vector2> unavailableCells)
    {
        List<Vector2> sortedCells = new List<Vector2>();
        List<List<Vector2>> rows = new List<List<Vector2>>();

        for (int y = 0; y < height; y++) 
        {
            rows.Add(new List<Vector2>());
        }

        for (int i = 0; i < unavailableCells.Count; i++)
        {
            int y = Mathf.RoundToInt(unavailableCells[i].y);

            rows[y].Add(unavailableCells[i]);
        }

        for (int i = 0; i < rows.Count; i++)
        {
            rows[i] = SortRow(rows[i]);
            sortedCells.AddRange(rows[i]);
        }

        unavailableCells = sortedCells;
    }

    List<Vector2> SortRow(List<Vector2> row)
    {
        Vector2 temporary;

        for (int j = 0; j <= row.Count - 2; j++)
        {
            for (int i = 0; i <= row.Count - 2; i++)
            {
                if (row[i].x > row[i + 1].x)
                {
                    temporary = row[i + 1];
                    row[i + 1] = row[i];
                    row[i] = temporary;
                }
            }
        }

        return row;
    }

    public void CalculateCellSize()
    {
        Camera cam = Camera.main;

        if (cam == null)
        {
            Debug.LogError("No se encontró la cámara principal!");
            return;
        }

        // Obtener dimensiones de la cámara en unidades de mundo
        float cameraHeight = 2f * cam.orthographicSize;
        float cameraWidth = cameraHeight * cam.aspect;

        // Restar el padding
        float usableHeight = cameraHeight - (padding * 2f);
        float usableWidth = cameraWidth - (padding * 2f);

        //Debug.Log($"Pantalla: {Screen.width}x{Screen.height} | Cámara: {cameraWidth:F2}x{cameraHeight:F2} unidades");

        // Calcular tamaño de celda según dimensión más restrictiva
        float cellSizeByWidth = usableWidth / width;
        float cellSizeByHeight = usableHeight / height;

        // Elegir el tamaño apropiado
        if (fillScreen)
        {
            // Llenar la pantalla (puede cortar parte del mapa)
            cellSize = Mathf.Max(cellSizeByWidth, cellSizeByHeight);
        }
        else
        {
            // Ajustar para que todo el mapa sea visible
            cellSize = Mathf.Min(cellSizeByWidth, cellSizeByHeight);
        }

        //Debug.Log($"Tamaño de celda calculado: {cellSize:F3} unidades | Grid: {width}x{height}");

        // Calcular dimensiones reales del mapa
        float mapWidth = width * cellSize;
        float mapHeight = height * cellSize;

        //Debug.Log($"Dimensiones del mapa: {mapWidth:F2}x{mapHeight:F2} unidades");

        // Centrar el mapa en la pantalla
        Vector3 centerPosition = new Vector3(
            -mapWidth / 2f,
            -mapHeight / 2f,
            0
        );

        transform.position = centerPosition;

        //Debug.Log($"Posición del grid: {transform.position}");

        // Información del área segura (útil para UI)
        Rect safeArea = Screen.safeArea;
        float safePercentX = (safeArea.width / Screen.width) * 100f;
        float safePercentY = (safeArea.height / Screen.height) * 100f;

        //Debug.Log($"Área segura: {safeArea.width}x{safeArea.height} ({safePercentX:F1}% x {safePercentY:F1}%)");

        // Información de cutouts (notches)
        var cutouts = Screen.cutouts;
        if (cutouts.Length > 0)
        {
            //Debug.Log($"Dispositivo tiene {cutouts.Length} cutout(s):");
            for (int i = 0; i < cutouts.Length; i++)
            {
                //Debug.Log($"  Cutout {i}: {cutouts[i].width}x{cutouts[i].height} px en posición ({cutouts[i].x}, {cutouts[i].y})");
            }
        }

        int centralCellX = Mathf.FloorToInt(width / 2f);
        int centralCellY = Mathf.FloorToInt(height / 2f);

        Vector2 position = FromCellToPosition(new Vector2(centralCellX, centralCellY));

        OnCellSizeCalculated?.Invoke(cellSize, position);
    }

    public List<Vector2> GetLimitPositions()
    {
        List<Vector2> leftRight = new List<Vector2>();
        List<Vector2> upDown = new List<Vector2>();

        for (int x = -1; x <= width+1; x += width + 1)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2 position = FromCellToPosition(new Vector2(x,y));
                leftRight.Add(position);
            }
        }

        for (int y = -1; y <= height + 1; y += height + 1)
        {
            for (int x = -1; x < width + 1; x++)
            {
                Vector2 position = FromCellToPosition(new Vector2(x, y));
                upDown.Add(position);
            }
        }

        List<Vector2> limits = new List<Vector2>();
        limits.AddRange(leftRight);
        limits.AddRange(upDown);

        return limits;
    }

    public void SetMapSize(Vector3 mapSize) 
    {
        width = Mathf.RoundToInt(mapSize.x);
        height = Mathf.RoundToInt(mapSize.y);
    }
}