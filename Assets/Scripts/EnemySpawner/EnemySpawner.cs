using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Object EnemyPrefab;
    
    public int SpawnDelay;

    private float _nextSpawnTime;

    void Start()
    {
        _nextSpawnTime = Time.time + SpawnDelay;
    }


    void Update()
    {
        if (Time.time >= _nextSpawnTime)
        {
            Instantiate(EnemyPrefab, transform.position, transform.rotation, transform.parent);
            _nextSpawnTime += SpawnDelay;
        }
    }
}
