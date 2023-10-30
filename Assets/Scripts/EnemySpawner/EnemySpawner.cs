using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Object EnemyPrefab;
    
    private float SpawnDelay;

    private int EnemySpawnCap;

    void Start()
    {
        EnemySpawnCap = Events.GetEnemySpawnCap();
        InvokeRepeating(nameof(SpawnEnemy), Events.GetInitialSpawnDelay(), Events.GetEnemySpawnDelay());
    }

    void SpawnEnemy()
    {
        if (GameObject.FindGameObjectsWithTag("Enemy").Length < EnemySpawnCap)
            Instantiate(EnemyPrefab, transform.position, transform.rotation, transform.parent);
    }
}
