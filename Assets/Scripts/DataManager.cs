using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{

    public static DataManager Instance;

    private float movespeed;
    private int health;

    private int levelProgess;


    public Color auraColor;
    private Color projectileColor;

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

        Events.OnSetProjectileColor += SetProjectileColor;
        Events.OnGetProjectileColor += GetProjectileColor;
        Events.OnSetAuraColor += SetAuraColor;
        Events.OnGetAuraColor += GetAuraColor;
    }

    private void Start()
    {
        auraColor = Color.cyan;

        projectileColor = Color.yellow;
    }

    private void OnDestroy()
    {
        Events.OnGetHealthPerm -= GetHealth;
        Events.OnSetHealthPerm -= SetHealth;
        Events.OnGetMovespeedPerm -= GetMovespeed;
        Events.OnSetMovespeedPerm -= SetMovespeed;

        Events.OnSetLevelProgress -= SetLevelProgress;
        Events.OnGetLevelProgress -= GetLevelProgress;


        Events.OnGetProjectileColor -= GetProjectileColor;
        Events.OnSetProjectileColor -= SetProjectileColor;
        Events.OnGetAuraColor -= GetAuraColor;
        Events.OnSetAuraColor -= SetAuraColor;
    }

    public void SetInitialStats()
    {
        movespeed = 4f;
        health = 100;
        levelProgess = 0;
    }


    public void SetLevelProgress(int amount) => levelProgess = amount;
    public int GetLevelProgress() => levelProgess;

    public float GetMovespeed() => movespeed;
    public void SetMovespeed(float amount) => movespeed = amount;
    public int GetHealth() => health;
    public void SetHealth(int amount) => health = amount;


    private Color GetProjectileColor() => projectileColor;
    private void SetProjectileColor(Color col) => projectileColor = col;
    private Color GetAuraColor() => auraColor;
    private void SetAuraColor(Color col)
    {
        auraColor = col;
    }
}
