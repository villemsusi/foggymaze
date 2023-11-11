using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ItemSpawner : MonoBehaviour
{
    public Stairs StairsPrefab;
    public Lootbox LootboxPrefab;
    public GameObject TurretItemPrefab;

    private Tilemap tilemap;
    private BoundsInt bounds;

    private List<Vector3> worldLocs;

    private int lootboxCount;
    private int turretDropCount;


    // Start is called before the first frame update
    void Start()
    {
        tilemap = GameObject.Find("Ground").GetComponent<Tilemap>();

        lootboxCount = Events.GetStartingLootboxCount();
        turretDropCount = Events.GetStartingTurretDropCount();

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
        int randPos = Random.Range(0, worldLocs.Count);
        Instantiate(StairsPrefab, worldLocs[randPos] + new Vector3(0, 0.3f, 0), Quaternion.identity, null);
        worldLocs.RemoveAt(randPos);
        for (int i = 0; i < lootboxCount; i++)
        {
            randPos = Random.Range(0, worldLocs.Count-i);
            Lootbox box = Instantiate(LootboxPrefab, worldLocs[randPos], Quaternion.identity, null);
            Events.AddInteractable(box.gameObject);

            worldLocs.RemoveAt(randPos);
        }

        for (int i = 0; i < turretDropCount; i++)
        {
            randPos = Random.Range(0, worldLocs.Count - i - lootboxCount);
            Instantiate(TurretItemPrefab, worldLocs[randPos], Quaternion.identity, null);

            worldLocs.RemoveAt(randPos);
        }

    }

}
