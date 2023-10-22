using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{

    public static DataManager Instance;

    private float movespeed = 4f;
    private int health = 100;

    private int levelProgess = 0;

    private void Awake()
    {

        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);


        Events.OnGetHealthPerm += GetHealth;
        Events.OnSetHealthPerm += SetHealth;
        Events.OnGetMovespeedPerm += GetMovespeed;
        Events.OnSetMovespeedPerm += SetMovespeed;

        Events.OnSetLevelProgress += SetLevelProgress;
        Events.OnGetLevelProgress += GetLevelProgress;
    }

    private void OnDestroy()
    {
        Events.OnGetHealthPerm -= GetHealth;
        Events.OnSetHealthPerm -= SetHealth;
        Events.OnGetMovespeedPerm -= GetMovespeed;
        Events.OnSetMovespeedPerm -= SetMovespeed;

        Events.OnSetLevelProgress -= SetLevelProgress;
        Events.OnGetLevelProgress -= GetLevelProgress;
    }


    public void SetLevelProgress(int amount) => levelProgess = amount;
    public int GetLevelProgress() => levelProgess;

    public float GetMovespeed() => movespeed;
    public void SetMovespeed(float amount) => movespeed = amount;
    public int GetHealth() => health;
    public void SetHealth(int amount) => health = amount;
}
