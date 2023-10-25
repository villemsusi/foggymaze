using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{

    public static DataManager Instance;

    private float movespeed;
    private int health;

    private int levelProgess;



    private int lootboxCount;
    private int turretDropCount;

    private int startingTurretCount;
    private float timer;

    private List<(Enemy, int)> Enemies;

    private void Awake()
    {

        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        SetInitialStats();
        Instance = this;
        DontDestroyOnLoad(gameObject);


        Events.OnGetHealthPerm += GetHealth;
        Events.OnSetHealthPerm += SetHealth;
        Events.OnGetMovespeedPerm += GetMovespeed;
        Events.OnSetMovespeedPerm += SetMovespeed;

        Events.OnSetLevelProgress += SetLevelProgress;
        Events.OnGetLevelProgress += GetLevelProgress;

        Events.OnGetStartingTurretCount += GetStartingTurretCount;
        Events.OnGetStartingLootboxCount += GetStartingLootboxCount;
        Events.OnGetStartingTurretDropCount += GetStartingTurretDropCount;

        Events.OnGetStageTimer += GetStageTimer;
    }

    private void OnDestroy()
    {
        Events.OnGetHealthPerm -= GetHealth;
        Events.OnSetHealthPerm -= SetHealth;
        Events.OnGetMovespeedPerm -= GetMovespeed;
        Events.OnSetMovespeedPerm -= SetMovespeed;

        Events.OnSetLevelProgress -= SetLevelProgress;
        Events.OnGetLevelProgress -= GetLevelProgress;

        Events.OnGetStartingTurretCount -= GetStartingTurretCount;
        Events.OnGetStartingLootboxCount -= GetStartingLootboxCount;
        Events.OnGetStartingTurretDropCount -= GetStartingTurretDropCount;

        Events.OnGetStageTimer -= GetStageTimer;
    }

    public void SetInitialStats()
    {
        movespeed = 4f;
        health = 10000;
        levelProgess = 0;
        timer = 1;
    }


    public void SetLevelProgress(int amount)
    {
        levelProgess = amount;
        turretDropCount = amount+2;
        lootboxCount = amount;
        startingTurretCount = amount;
    }
    public int GetLevelProgress() => levelProgess;

    public int GetStartingTurretCount() => startingTurretCount;
    public int GetStartingLootboxCount() => lootboxCount;
    public int GetStartingTurretDropCount() => turretDropCount;
    public float GetStageTimer() => timer;


    public float GetMovespeed() => movespeed;
    public void SetMovespeed(float amount) => movespeed = amount;
    public int GetHealth() => health;
    public void SetHealth(int amount) => health = amount;
}
