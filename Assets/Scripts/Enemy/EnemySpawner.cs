using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemySpawner : MonoBehaviour
{
    public List<GameObject> EnemyPrefabs = new();
    
    private float SpawnDelay;
    private int EnemySpawnCap;

    private Tilemap tilemap;
    private BoundsInt bounds;
    private List<Vector3> worldLocs;

    void Start()
    {
        tilemap = GameObject.Find("Ground").GetComponent<Tilemap>();
        worldLocs = new List<Vector3>();
        bounds = tilemap.cellBounds;

        foreach (var pos in bounds.allPositionsWithin)
        {
            Vector3Int localLoc = new Vector3Int(pos.x, pos.y, pos.z);
            if (tilemap.HasTile(localLoc))
            {
                Vector3 offsetPos = tilemap.CellToWorld(localLoc);
                offsetPos.x += 0.5f;
                offsetPos.y += 0.2f;
                worldLocs.Add(offsetPos);
            }
        }

        EnemySpawnCap = Events.GetEnemySpawnCap();
        InvokeRepeating(nameof(SpawnEnemy), Events.GetInitialSpawnDelay(), Events.GetEnemySpawnDelay());
    }

    void SpawnEnemy()
    {
        if (GameObject.FindGameObjectsWithTag("Enemy").Length < EnemySpawnCap)
        {
            List<Vector3> suitableWorldLocs = new();
            Vector3 pos = Events.GetPlayerPosition();
            foreach (Vector3 loc in worldLocs)
            {
                if
                (
                    
                    Mathf.Abs(loc.x - pos.x) >= 5 &&
                    Mathf.Abs(loc.x - pos.x) <= 10 &&
                    Mathf.Abs(loc.y - pos.y) >= 5 &&
                    Mathf.Abs(loc.y - pos.y) <= 10
                )
                {
                    suitableWorldLocs.Add(loc);
                }
            }

            if (suitableWorldLocs.Count > 0)
            {
                int randPos = Random.Range(0, suitableWorldLocs.Count);
                int randEnemy = Random.Range(0, EnemyPrefabs.Count);
                Instantiate(EnemyPrefabs[randEnemy], suitableWorldLocs[randPos], Quaternion.identity, null);
            }
        }
    }
}
