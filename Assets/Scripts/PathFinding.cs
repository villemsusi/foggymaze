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
    BoundsInt bounds;

    float speed = 1;

    public TileBase roadTile;
    public TileBase groundTile;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        tilemap.CompressBounds();
        bounds = tilemap.cellBounds;


        CreateGrid();
        AStar = new AStar(grid, bounds.size.x, bounds.size.y);
    }
    // Function for mapping out the grid for A*
    public void CreateGrid()
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

        // Get path start coordinates
        Vector3 worldStart = transform.position;
        Vector3Int gridPosStart = tilemap.WorldToCell(worldStart);
        start = new Vector2Int(gridPosStart.x, gridPosStart.y);

        // Get path end coordinates
        Vector3 worldEnd = Player.Instance.transform.position;
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
        
        // If there is no viable path and enemy is not immediately next to the player, don't try to draw the path
        if (roadPath == null && Vector3.Distance(transform.position, Player.Instance.transform.position) > 1f)
            return;

        // Movement speed of enemy
        var step = speed * Time.deltaTime;
        var tilesize = 0.5f;

        // If there is a viable path and the enemy is atleast 1 unit away from the player
        // Draw the path and move the enemy towards the next node in the path
        if (Vector3.Distance(transform.position, Player.Instance.transform.position) > 1f && roadPath != null)
        {
            DrawRoad(); // Development aiding function - should be deleted for production
            Vector3 nextNode = tilemap.CellToWorld(roadPath[1].SpotToVector());
            nextNode.x += tilesize;
            nextNode.y += tilesize;
            transform.position = Vector3.MoveTowards(transform.position, nextNode, step);

            animator.SetFloat("X", nextNode.x-transform.position.x);
            animator.SetFloat("Y", nextNode.y-transform.position.y);

            animator.SetBool("isWalking", true);
        }
        // If the enemy is closer than 1 unit to the player
        // Move the enemy directly towards the player position
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, Player.Instance.transform.position, step);
        }
        

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
