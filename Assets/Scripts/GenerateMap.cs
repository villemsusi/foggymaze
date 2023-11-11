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

    public int Width = 30;
    public int Height = 30;
    private int xMin;
    private int yMin;

    public Player PlayerPrefab;

    

    Vector2Int[] dirs = { new Vector2Int(-1, 0), new Vector2Int(0, -1), new Vector2Int(0, 1), new Vector2Int(1, 0) };

    private void Awake()
    {
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
