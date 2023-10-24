using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemySpawner : MonoBehaviour
{
    public Object EnemyPrefab;
    public Tilemap Tilemap;
    public int SpawnDelay;

    private float _nextSpawnTime;

    void Start()
    {
        _nextSpawnTime = Time.time + SpawnDelay;
        PathFinding pathFinding = EnemyPrefab.GetComponent<PathFinding>();
        pathFinding.tilemap = Tilemap;
    }


    void Update()
    {
        if (Time.time >= _nextSpawnTime)
        {
            Instantiate(EnemyPrefab, transform.position, transform.rotation);
            _nextSpawnTime += SpawnDelay;
        }
    }
}
