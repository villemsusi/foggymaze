using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "FoggyMaze/InitialStats")]
public class InitialStatData : ScriptableObject
{
    public int health;
    public float movespeed;
    public float timer;
    public float fogScale;

    public int enemySpawnCap;
    public int startingTurretCount;
    public int turretDropCount;
    public int lootboxCount;
    public float enemySpawnDelay;
    public float initialSpawnDelay;
    public float enemyHealthFactor;
    public float enemySpeedFactor;


}
