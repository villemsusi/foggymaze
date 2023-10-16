using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathFinding : MonoBehaviour
{
    
    private Vector3Int[,] grid;

    AStar AStar;
    List<Spot> roadPath = new List<Spot>();
    BoundsInt bounds;

    public Tilemap tilemap;
    public TileBase roadTile;
    public TileBase groundTile;

    

    void Start()
    {
        tilemap.CompressBounds();
        bounds = tilemap.cellBounds;


        CreateGrid();
        AStar = new AStar(grid, bounds.size.x, bounds.size.y);
    }
    // Function for mapping out the grid for A*
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

    
    public Vector2Int start;
    public Vector2Int end;

    void Update()
    {

    }

    public List<Spot> GetPath()
    {
        // Get path start coordinates
        Vector3 worldStart = transform.position;
        Vector3Int gridPosStart = tilemap.WorldToCell(worldStart);
        start = new Vector2Int(gridPosStart.x, gridPosStart.y);

        // Get path end coordinates
        Vector3 worldEnd = Events.GetPlayerPosition();
        Vector3Int gridPosEnd = tilemap.WorldToCell(worldEnd);
        end = new Vector2Int(gridPosEnd.x, gridPosEnd.y);

        // If a path is drawn, erase it
        // Development aiding function - should be deleted for production
        if (roadPath != null)
        {
            ClearRoad();
        }
        // A* algorithm
        roadPath = AStar.CreatePath(grid, end, start, 1000);

        if (roadPath != null)
            DrawRoad();
        return roadPath;
    }

    public Vector3 GetTilemapCoords(Spot nextNode)
    {
        return tilemap.CellToWorld(nextNode.SpotToVector());
    }

    // Place road tiles based on the current path
    // Development aiding function - should be deleted for production
    private void DrawRoad()
    {
        for (int i = 0; i < roadPath.Count; i++)
        {
            tilemap.SetTile(new Vector3Int(roadPath[i].X, roadPath[i].Y, 0), roadTile);
        }
    }

    // Place ground tiles on the previous path
    // Development aiding function - should be deleted for production
    private void ClearRoad()
    {
        for (int i = 0; i < roadPath.Count; i++)
        {
            tilemap.SetTile(new Vector3Int(roadPath[i].X, roadPath[i].Y, 0), groundTile);
        }
    }
}
