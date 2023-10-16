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
    public Sprite Icon;
    public Tower TowerPrefab;

    public Projectile ProjectilePrefab;
    public int ProjDamage;
    public float ProjCooldown;
    public float ProjSpeed;
}
