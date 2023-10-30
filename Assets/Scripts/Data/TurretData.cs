using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FoggyMaze/Turret")]
public class TurretData : ScriptableObject
{
    public string Name;
    public string Shortcut;

    public string Type;

    public float ShotRadius;
    public Texture Texture;
    public Turret TurretPrefab;
    public Turret NextUpgrade;

    public Projectile ProjectilePrefab;
    public int ProjDamage;
    public float ProjCooldown;
    public float ProjSpeed;
    public float Kickback;

    public int MaxAmmo;
    public int ReloadCost;
}
