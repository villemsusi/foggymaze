using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathFinding : MonoBehaviour
{
    public Tilemap tilemap;
    public Vector3Int[,] grid;
    AStar AStar;
    List<Spot> roadPath = new List<Spot>();
    new Camera camera;
    BoundsInt bounds;

    float speed = 1;

    public TileBase roadTile;
    public TileBase groundTile;
    // Start is called before the first frame update
    void Start()
    {
        tilemap.CompressBounds();
        bounds = tilemap.cellBounds;


        CreateGrid();
        AStar = new AStar(grid, bounds.size.x, bounds.size.y);
    }
    public void CreateGrid()
    {
        grid = new Vector3Int[bounds.size.x, bounds.size.y];
        for (int x = bounds.xMin, i = 0; i < (bounds.size.x); x++, i++)
        {
            for (int y = bounds.yMin, j = 0; j < (bounds.size.y); y++, j++)
            {
                if (tilemap.HasTile(new Vector3Int(x, y, 0)))
                {
                    grid[i, j] = new Vector3Int(x, y, 0);
                }
                else
                {
                    grid[i, j] = new Vector3Int(x, y, 1);
                }
            }
        }
    }

    // Update is called once per frame
    public Vector2Int start;
    public Vector2Int end;
    void Update()
    {

        Vector3 worldStart = transform.position;
        Vector3Int gridPosStart = tilemap.WorldToCell(worldStart);
        start = new Vector2Int(gridPosStart.x, gridPosStart.y);

        Vector3 worldEnd = Player.Instance.transform.position;
        Vector3Int gridPosEnd = tilemap.WorldToCell(worldEnd);
        end = new Vector2Int(gridPosEnd.x, gridPosEnd.y);


        if (roadPath != null)
            ClearRoad();
        roadPath = AStar.CreatePath(grid, start, end, 1000);
        if (roadPath == null)
            return;
        DrawRoad();
        var step = speed * Time.deltaTime;
        
        var tilesize = 0.3f;
        Vector3 nextNode = roadPath[0].SpotToVector();
        Debug.Log(nextNode);
        nextNode.x += tilesize;
        nextNode.y += tilesize;
        transform.position = Vector3.MoveTowards(transform.position, nextNode, step);
    }

    private void DrawRoad()
    {
        for (int i = 0; i < roadPath.Count; i++)
        {
            tilemap.SetTile(new Vector3Int(roadPath[i].X, roadPath[i].Y, 0), roadTile);
        }
    }

    private void ClearRoad()
    {
        for (int i = 0; i < roadPath.Count; i++)
        {
            tilemap.SetTile(new Vector3Int(roadPath[i].X, roadPath[i].Y, 0), groundTile);
        }
    }
}
