using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FoggyMaze/Tower")]
public class TowerData : ScriptableObject
{
    public string Name;
    public int Cost;
    public float ShotRadius;
    public string Shortcut;
    public Texture Texture;
    public Tower TowerPrefab;
    public Tower NextUpgrade;

    public Projectile ProjectilePrefab;
    public int ProjDamage;
    public float ProjCooldown;
    public float ProjSpeed;

    public int MaxAmmo;
    public int ReloadCost;
}
