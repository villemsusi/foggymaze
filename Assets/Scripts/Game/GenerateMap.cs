using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
//using Unity.AI.Navigation;
using NavMeshPlus.Components;
using UnityEngine.UIElements;

public class GenerateMap : MonoBehaviour
{
    // MAP GENERATING VALUES
    public GameObject NavMesh;
    private NavMeshSurface navMeshSurface;

    public List<Tile> GroundTiles_Reg;
    public Tile WallTile_Reg;
    public List<Tile> GroundTiles_Green;
    public Tile WallTile_Green;
    public List<Tile> GroundTiles_Red;
    public Tile WallTile_Red;

    private string STATE = "REGULAR";

    public Tilemap Ground;
    public Tilemap Walls;

    private BoundsInt bounds;


    public int Width = 30;
    public int Height = 30;
    private int xMin;
    private int yMin;

    public Player PlayerPrefab;

    Vector2Int[] dirs = { new Vector2Int(-1, 0), new Vector2Int(0, -1), new Vector2Int(0, 1), new Vector2Int(1, 0) };


    // ITEM SPAWNING VALUES
    public GameObject StairsPrefab;
    public GameObject LootboxPrefab;
    public GameObject TurretItemPrefab;
    public GameObject WebTrapPrefab;

    public Enemy RegularEnemyPrefab;
    public Enemy BeefyEnemyPrefab;
    public Enemy ShieldedEnemyPrefab;

    private List<Vector3> worldLocs;

    private int lootboxCount;
    private int turretDropCount;
    private int trapCount;


    private void Awake()
    {
        STATE = Random.value <= 0.66 ? (Random.value <= 0.33 ? "REGULAR" : "RED") : "GREEN";

        navMeshSurface = NavMesh.GetComponent<NavMeshSurface>();

        xMin = -1 * Width / 2;
        yMin = -1 * Height / 2;

        bounds = new BoundsInt(new Vector3Int(xMin, yMin, 0), new Vector3Int(-2*xMin, -2 * yMin, 1));

        SetGroundTile(Vector3Int.zero);
        GenMaze(Vector3Int.zero);
        foreach (var pos in bounds.allPositionsWithin)
        {
            // If pos at edge of maze, set wall
            if (pos.x == xMin || pos.x == -1 * xMin - 1 || pos.y == yMin || pos.y == -1 * yMin - 1)
            {
                SetWallTile(pos);
                continue;
            }
            if (!Walls.HasTile(pos) && !Ground.HasTile(pos))
            {
                float rand = Random.value;
                if (rand <= 0.8)
                    SetWallTile(pos);
                else
                    SetGroundTile(pos);
                    
            }
        }

        

        Instantiate(PlayerPrefab, new Vector3(0.5f, 0.5f, 0), Quaternion.identity, null);

    }



    void Start()
    {
        navMeshSurface.BuildNavMeshAsync();



        lootboxCount = Events.GetStartingLootboxCount();
        turretDropCount = Events.GetStartingTurretDropCount();
        trapCount = Events.GetTrapCount();

        worldLocs = new List<Vector3>();

        foreach (var pos in bounds.allPositionsWithin)
        {
            Vector3Int localLoc = new Vector3Int(pos.x, pos.y, pos.z);
            if (Ground.HasTile(localLoc))
            {
                worldLocs.Add(Ground.CellToWorld(localLoc));
            }

        }
        SpawnItem(StairsPrefab, false);

        for (int i = 0; i < trapCount; i++)
            SpawnItem(WebTrapPrefab, false);


        for (int i = 0; i < lootboxCount; i++)
            SpawnItem(LootboxPrefab, true);

        for (int i = 0; i < turretDropCount; i++)
            SpawnItem(TurretItemPrefab, false);

    }

    void SpawnItem(GameObject prefab, bool interactable)
    {
        int randPos = Random.Range(0, worldLocs.Count);
        GameObject item;
        if (prefab.name == "Stairs")
            item = Instantiate(prefab, worldLocs[randPos] + new Vector3(0.5f, 0.5f, 0), Quaternion.identity, null);
        else
            item = Instantiate(prefab, worldLocs[randPos] + new Vector3(Random.Range(0.3f, 0.7f), Random.value/2, 0), Quaternion.identity, null);
        if (interactable)
        {
            Events.AddInteractable(item);
        }
        worldLocs.RemoveAt(randPos);
    }

    void GenMaze(Vector3Int pos)
    {

        reshuffle(dirs);

        foreach (Vector2Int dir in dirs)
        {
            Vector3Int newPos = new(pos.x + dir.x, pos.y + dir.y, pos.z);

            if (newPos.x == 0 && newPos.y == 0)
            {
                SetGroundTile(newPos);
                GenMaze(newPos);
                
            }
            else if (IsGoodPath(newPos)) {
                SetGroundTile(newPos);
                GenMaze(newPos);
            }
        }
    }

    bool IsGoodPath(Vector3Int pos)
    {
        if (pos.x <= xMin || pos.x >= -1 * xMin - 1 || pos.y <= yMin || pos.y >= -1 * yMin - 1)
            return false;
        if (Ground.HasTile(pos))
            return false;

        int count = 0;
        foreach (Vector2Int dir in dirs)
        {
            Vector3Int newPos = new(pos.x + dir.x, pos.y + dir.y, pos.z);
            if (Ground.HasTile(newPos))
                count++;
        }

        if (count > 1)
            return false;


        return true;
    }


    private void SetWallTile(Vector3Int pos)
    {
        switch(STATE)
        {
            case "REGULAR":
                Walls.SetTile(pos, WallTile_Reg);
                break;
            case "GREEN":
                Walls.SetTile(pos, WallTile_Green);
                break;
            case "RED":
                Walls.SetTile(pos, WallTile_Red);
                break;
            default: break;
        }
        
    }

    private void SetGroundTile(Vector3Int pos)
    {
        float rand = Random.value;
        switch (STATE)
        {
            case "REGULAR":
                if (rand <= 0.8)
                    Ground.SetTile(pos, GroundTiles_Reg[0]);
                else if (rand <= 0.9)
                    Ground.SetTile(pos, GroundTiles_Reg[1]);
                else
                    Ground.SetTile(pos, GroundTiles_Reg[2]);
                break;
            case "GREEN":
                if (rand <= 0.8)
                    Ground.SetTile(pos, GroundTiles_Green[0]);
                else if (rand <= 0.9)
                    Ground.SetTile(pos, GroundTiles_Green[1]);
                else
                    Ground.SetTile(pos, GroundTiles_Green[2]);
                break;
            case "RED":
                if (rand <= 0.8)
                    Ground.SetTile(pos, GroundTiles_Red[0]);
                else if (rand <= 0.9)
                    Ground.SetTile(pos, GroundTiles_Red[1]);
                else
                    Ground.SetTile(pos, GroundTiles_Red[2]);
                break;
            default: break;
        }
        
    }

    void reshuffle(Vector2Int[] positions)
    {
        for (int t = 0; t < positions.Length; t++)
        {
            Vector2Int tmp = positions[t];
            int r = Random.Range(t, positions.Length);
            positions[t] = positions[r];
            positions[r] = tmp;
        }
    }

}
