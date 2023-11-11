using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
//using Unity.AI.Navigation;
using NavMeshPlus.Components;

public class GenerateMap : MonoBehaviour
{
    // MAP GENERATING VALUES
    public GameObject NavMesh;
    private NavMeshSurface navMeshSurface;

    public List<Tile> GroundTiles;
    public Tile WallTile;

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
    public Stairs StairsPrefab;
    public EnemySpawner EnemySpawnerPrefab;
    public Lootbox LootboxPrefab;
    public GameObject TurretItemPrefab;
    public Enemy RegularEnemyPrefab;
    public Enemy BeefyEnemyPrefab;

    private List<Vector3> worldLocs;

    private int lootboxCount;
    private int turretDropCount;


    private void Awake()
    {
        navMeshSurface = NavMesh.GetComponent<NavMeshSurface>();

        xMin = -1 * Width / 2;
        yMin = -1 * Height / 2;

        bounds = new BoundsInt(new Vector3Int(xMin, yMin, 0), new Vector3Int(-2*xMin, -2 * yMin, 1));


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

        worldLocs = new List<Vector3>();

        foreach (var pos in bounds.allPositionsWithin)
        {
            Vector3Int localLoc = new Vector3Int(pos.x, pos.y, pos.z);
            if (Ground.HasTile(localLoc))
            {
                worldLocs.Add(Ground.CellToWorld(localLoc));
            }

        }
        int randPos = Random.Range(0, worldLocs.Count);
        Instantiate(StairsPrefab, worldLocs[randPos] + new Vector3(0.5f, 0.5f, 0), Quaternion.identity, null);
        worldLocs.RemoveAt(randPos);

        randPos = Random.Range(0, worldLocs.Count);
        EnemySpawner spawner_regular = Instantiate(EnemySpawnerPrefab, worldLocs[randPos] + new Vector3(0.5f, 0.5f, 0), Quaternion.identity, null);
        spawner_regular.EnemyPrefab = RegularEnemyPrefab;
        worldLocs.RemoveAt(randPos);

        randPos = Random.Range(0, worldLocs.Count);
        EnemySpawner spawner_beefy = Instantiate(EnemySpawnerPrefab, worldLocs[randPos] + new Vector3(0.5f, 0.5f, 0), Quaternion.identity, null);
        spawner_beefy.EnemyPrefab = BeefyEnemyPrefab;
        worldLocs.RemoveAt(randPos);

        for (int i = 0; i < lootboxCount; i++)
        {
            randPos = Random.Range(0, worldLocs.Count - i);
            Lootbox box = Instantiate(LootboxPrefab, worldLocs[randPos] + new Vector3(0.5f, 0.2f, 0), Quaternion.identity, null);
            Events.AddInteractable(box.gameObject);

            worldLocs.RemoveAt(randPos);
        }

        for (int i = 0; i < turretDropCount; i++)
        {
            randPos = Random.Range(0, worldLocs.Count - i - lootboxCount);
            Instantiate(TurretItemPrefab, worldLocs[randPos] + new Vector3(0.5f, 0.2f, 0), Quaternion.identity, null);

            worldLocs.RemoveAt(randPos);
        }

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
        Walls.SetTile(pos, WallTile);
    }

    private void SetGroundTile(Vector3Int pos)
    {
        float rand = Random.value;
        if (rand <= 0.8)
            Ground.SetTile(pos, GroundTiles[0]);
        else if (rand <= 0.9)
            Ground.SetTile(pos, GroundTiles[1]);
        else
            Ground.SetTile(pos, GroundTiles[2]);
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
