using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ItemSpawner : MonoBehaviour
{

    public Tilemap tilemap;
    private BoundsInt bounds;
    public Lootbox LootboxPrefab;
    public GameObject TurretItemPrefab;

    private List<Vector3> worldLocs;

    private int lootboxCount;
    private int turretDropCount;
    // Start is called before the first frame update
    void Start()
    {
        lootboxCount = Events.GetLootboxCount();
        turretDropCount = Events.GetTurretDropCount();

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
            int randPos = Random.Range(0, worldLocs.Count-i);
            Lootbox box = Instantiate(LootboxPrefab, worldLocs[randPos], Quaternion.identity, null);
            Events.AddInteractable(box.gameObject);

            worldLocs.RemoveAt(randPos);
        }

        for (int i = 0; i < turretDropCount; i++)
        {
            int randPos = Random.Range(0, worldLocs.Count - i - lootboxCount);
            Instantiate(TurretItemPrefab, worldLocs[randPos], Quaternion.identity, null);

            worldLocs.RemoveAt(randPos);
        }

    }

}
