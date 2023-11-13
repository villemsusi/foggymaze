using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{

    public static DataManager Instance;

    public InitialStatData InitialStats;

    private float movespeed;
    private int health;

    private int levelProgess;


    public Color auraColor;
    private Color projectileColor;


    private int lootboxCount;
    private int turretDropCount;

    private int startingTurretCount;
    private float timer;

    private int enemySpawnCap;
    private float enemySpawnDelay;
    private float initialSpawnDelay;

    private float enemyMoveSpeedFactor;
    private float enemyHealthFactor;

    public AudioClipGroup BckgrMusic;
    public AudioClipGroup BuildAudio;
    public AudioClipGroup UpgradeAudio;
    public AudioClipGroup ReloadAudio;
    public AudioClipGroup ShootAudio;
    public AudioClipGroup DenyAudio;
    public AudioClipGroup EnemyDeathAudio;
    public AudioClipGroup WaterDropAudio;
    public AudioClipGroup WalkAudio;
    public AudioClipGroup OpenBoxAudio;


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

        Events.OnGetInitialSpawnDelay += GetInitialSpawnDelay;
        Events.OnGetEnemySpawnDelay += GetEnemySpawnDelay;
        Events.OnGetEnemySpawnCap += GetEnemySpawnCap;

        Events.OnGetEnemyHealthFactor += GetEnemyHealthFactor;
        Events.OnGetEnemyMovespeedFactor += GetEnemyMovespeedFactor;

        Events.OnSetProjectileColor += SetProjectileColor;
        Events.OnGetProjectileColor += GetProjectileColor;
        Events.OnSetAuraColor += SetAuraColor;
        Events.OnGetAuraColor += GetAuraColor;

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

        Events.OnGetInitialSpawnDelay -= GetInitialSpawnDelay;
        Events.OnGetEnemySpawnDelay -= GetEnemySpawnDelay;
        Events.OnGetEnemySpawnCap -= GetEnemySpawnCap;

        Events.OnGetEnemyHealthFactor -= GetEnemyHealthFactor;
        Events.OnGetEnemyMovespeedFactor -= GetEnemyMovespeedFactor;


        Events.OnGetProjectileColor -= GetProjectileColor;
        Events.OnSetProjectileColor -= SetProjectileColor;
        Events.OnGetAuraColor -= GetAuraColor;
        Events.OnSetAuraColor -= SetAuraColor;

        Events.OnGetStartingTurretCount -= GetStartingTurretCount;
        Events.OnGetStartingLootboxCount -= GetStartingLootboxCount;
        Events.OnGetStartingTurretDropCount -= GetStartingTurretDropCount;

        Events.OnGetStageTimer -= GetStageTimer;
    }

    private void Start()
    {
        auraColor = Color.blue;
        projectileColor = Color.red;

        BckgrMusic.Play();

    }

    public void SetInitialStats()
    {
        levelProgess = 1;

        movespeed = InitialStats.movespeed;
        health = InitialStats.health;
        
        startingTurretCount = InitialStats.startingTurretCount;
        turretDropCount = InitialStats.turretDropCount;
        lootboxCount = InitialStats.lootboxCount;

        initialSpawnDelay = InitialStats.initialSpawnDelay;
        enemySpawnDelay = InitialStats.enemySpawnDelay;
        enemySpawnCap = InitialStats.enemySpawnCap;
        
        enemyHealthFactor = InitialStats.enemyHealthFactor;
        enemyMoveSpeedFactor = InitialStats.enemySpeedFactor;

        timer = InitialStats.timer;
    }

    
    public void SetLevelProgress(int amount)
    {
        
        levelProgess = amount;
        turretDropCount = amount + 3;
        lootboxCount = amount + 10;
        startingTurretCount = amount;

        timer += 5;

        enemySpawnCap = Mathf.Min(50, enemySpawnCap + amount);
        enemySpawnDelay = Mathf.Max(1.5f, enemySpawnDelay - 0.1f);

        enemyHealthFactor *= 1.05f;
        enemyMoveSpeedFactor *= 1.05f;
    }
    public int GetLevelProgress() => levelProgess;

    public int GetStartingTurretCount() => startingTurretCount;
    public int GetStartingLootboxCount() => lootboxCount;
    public int GetStartingTurretDropCount() => turretDropCount;
    public float GetStageTimer() => timer;
    public int GetEnemySpawnCap() => enemySpawnCap;
    public float GetEnemySpawnDelay() => enemySpawnDelay;
    public float GetInitialSpawnDelay() => initialSpawnDelay;

    private float GetEnemyMovespeedFactor() => enemyMoveSpeedFactor;
    private float GetEnemyHealthFactor() => enemyHealthFactor;



    public float GetMovespeed() => movespeed;
    public void SetMovespeed(float amount) => movespeed = amount;
    public int GetHealth() => health;
    public void SetHealth(int amount) => health = amount;


    private Color GetProjectileColor() => projectileColor;
    private void SetProjectileColor(Color col) => projectileColor = col;
    private Color GetAuraColor() => auraColor;
    private void SetAuraColor(Color col) => auraColor = col;
}
