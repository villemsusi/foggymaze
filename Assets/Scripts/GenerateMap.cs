using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GenerateMap : MonoBehaviour
{
    public List<Tile> GroundTiles;
    public Tile WallTile;

    public Tilemap Ground;
    public Tilemap Walls;

    private BoundsInt bounds;

    public Player PlayerPrefab;

    private int xMin = -25;
    private int yMin = -25;

    private void Awake()
    {
        bounds = new BoundsInt(new Vector3Int(xMin, yMin, 0), new Vector3Int(Mathf.Abs(2*xMin), Mathf.Abs(2 * yMin), 1));

        List<Vector3Int> positions = new List<Vector3Int>();
        foreach (var pos in bounds.allPositionsWithin)
        {
            positions.Add(pos);
        }

        reshuffle(positions);

        foreach (var pos in positions)
        {
            Vector3Int localLoc = new Vector3Int(pos.x, pos.y, pos.z);

            SetTile(localLoc);
        }

        Instantiate(PlayerPrefab, new Vector3(0.5f, 0.5f, 0), Quaternion.identity, null);

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

    private void SetTile(Vector3Int pos)
    {
        if (pos.x == 0 && pos.y == 0)
        {
            SetGroundTile(pos);
            return;
        }
        if (pos.x == xMin || pos.x == Mathf.Abs(xMin) - 1 || pos.y == yMin || pos.y == Mathf.Abs(yMin) - 1)
        {
            Walls.SetTile(pos, WallTile);
            return;
        }

        int adj = CheckAdjacent(pos);
        float cutoff;

        if (adj == 0)
            cutoff = 0.55f;
        else if (adj == 1)
            cutoff = 0.4f;
        else
            cutoff = 0.3f;

        float rand = Random.value;
        if (rand <= cutoff)
        {
            Walls.SetTile(pos, WallTile);
            return;
        }
        SetGroundTile(pos);
    }


    private int CheckAdjacent(Vector3Int pos)
    {
        int nr = 0;
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (Walls.HasTile(pos + new Vector3Int(i, j, 0)))
                {
                    nr += 1;
                }
            }
        }

        return nr;
    }


    void reshuffle(List<Vector3Int> positions)
    {
        for (int t = 0; t < positions.Count; t++)
        {
            Vector3Int tmp = positions[t];
            int r = Random.Range(t, positions.Count);
            positions[t] = positions[r];
            positions[r] = tmp;
        }
    }
}
