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
                float positionX = transform.position.x + (cellSize * (x + 1 / 2f));
                float positionY = transform.position.y + (cellSize * (y + 1 / 2f));

                Vector2 position = new Vector2(positionX, positionY);

                //  Draw a cell with its center at grid positions, with the desired size
                Gizmos.DrawWireCube(position, Vector3.one * cellSize);
            }
        }
    }
}
