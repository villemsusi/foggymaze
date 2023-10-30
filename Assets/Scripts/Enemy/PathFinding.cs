using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathFinding : MonoBehaviour
{
    private Tilemap tilemap;



    private Vector3Int[,] grid;
    AStar AStar;
    List<Spot> roadPath = new List<Spot>();
    BoundsInt bounds;
    private Vector2Int start;
    private Vector2Int end;


    private int callCounter = 0;


    void Start()
    {
        tilemap = GameObject.Find("Ground").GetComponent<Tilemap>();

        tilemap.CompressBounds();
        bounds = tilemap.cellBounds;


        CreateGrid();
        AStar = new AStar(grid, bounds.size.x, bounds.size.y);
        LogCalls();
    }



    void LogCalls()
    {
        if (GameObject.FindGameObjectsWithTag("Enemy").Length != 0)
            Debug.Log("AVG pathfind calls per enemy per second: " + callCounter / 5f / GameObject.FindGameObjectsWithTag("Enemy").Length);
        callCounter = 0;
        Debug.Log(GameObject.FindGameObjectsWithTag("Enemy").Length);
        Invoke(nameof(LogCalls), 5);
    }

    public List<Spot> GetPath(Vector3 startPos)
    {
        callCounter += 1;
        // Get path start coordinates
        Vector3 worldStart = startPos;
        Vector3Int gridPosStart = tilemap.WorldToCell(worldStart);
        start = new Vector2Int(gridPosStart.x, gridPosStart.y);

        // Get path end coordinates
        Vector3 worldEnd = Events.GetPlayerPosition();
        Vector3Int gridPosEnd = tilemap.WorldToCell(worldEnd);
        end = new Vector2Int(gridPosEnd.x, gridPosEnd.y);

        // A* algorithm
        roadPath = AStar.CreatePath(grid, end, start, 1000);

        return roadPath;
    }

    public Vector3 GetTilemapCoords(Spot nextNode)
    {
        return tilemap.CellToWorld(nextNode.SpotToVector());
    }



    private void CreateGrid()
    {
        grid = new Vector3Int[bounds.size.x, bounds.size.y];
        for (int x = bounds.xMin, i = 0; i < (bounds.size.x); x++, i++)
        {
            for (int y = bounds.yMin, j = 0; j < (bounds.size.y); y++, j++)
            {
                // If there is a walkable ground tile on coordinates x, y
                // Grid value is 0
                if (tilemap.HasTile(new Vector3Int(x, y, 0)))
                {
                    grid[i, j] = new Vector3Int(x, y, 0);
                }
                // If there isn't walkable terrain on coords x, y
                // Grid value is 1
                else
                {
                    grid[i, j] = new Vector3Int(x, y, 1);
                }
            }
        }
    }

}
