using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FoggyMaze/Enemy")]
public class EnemyData : ScriptableObject
{
    public int Health;
    public int Damage;
    public float Speed;
    public float KnockbackAmount;
    public Enemy EnemyPrefab;

    public bool Teleporting;
}
