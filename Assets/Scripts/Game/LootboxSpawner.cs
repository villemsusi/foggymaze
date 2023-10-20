using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LootboxSpawner : MonoBehaviour
{

    public Tilemap tilemap;
    private BoundsInt bounds;
    public Lootbox LootboxPrefab;
    private List<Vector3> worldLocs;

    private int lootboxCount;
    // Start is called before the first frame update
    void Start()
    {
        lootboxCount = 5;

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
        for (int i = 0; i < lootboxCount; i++)
        {
            int randPos = UnityEngine.Random.Range(0, worldLocs.Count-i);
            Instantiate(LootboxPrefab, worldLocs[randPos], Quaternion.identity, null);

            worldLocs.RemoveAt(randPos);
        }
        
        
    }

}
