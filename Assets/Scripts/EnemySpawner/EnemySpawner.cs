using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemySpawner : MonoBehaviour
{
    public List<GameObject> EnemyPrefabs;
    
    private float SpawnDelay;
    private int EnemySpawnCap;

    private Tilemap tilemap;
    private BoundsInt bounds;
    private List<Vector3> worldLocs;
    private Player player;

    void Start()
    {
        tilemap = GameObject.Find("Ground").GetComponent<Tilemap>();
        worldLocs = new List<Vector3>();
        bounds = tilemap.cellBounds;
        player = GameObject.Find("Player").GetComponent<Player>();

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
            List<Vector3> suitableWorldLocs = new List<Vector3>();
            foreach (Vector3 loc in worldLocs)
            {
                if
                (
                    Mathf.Abs(loc.x - player.transform.position.x) >= 5 &&
                    Mathf.Abs(loc.x - player.transform.position.x) <= 10 &&
                    Mathf.Abs(loc.y - player.transform.position.y) >= 5 &&
                    Mathf.Abs(loc.y - player.transform.position.y) <= 10
                )
                {
                    suitableWorldLocs.Add(loc);
                }
            }

            if (suitableWorldLocs.Count > 0)
            {
                int randPos = Random.Range(0, suitableWorldLocs.Count);
                int randEnemy = Random.Range(0, EnemyPrefabs.Count);
                GameObject newEnemy = Instantiate(EnemyPrefabs[randEnemy], suitableWorldLocs[randPos], Quaternion.identity, null);
            }
        }
    }
}
