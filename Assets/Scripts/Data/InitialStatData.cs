using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "FoggyMaze/InitialStats")]
public class InitialStatData : ScriptableObject
{
    public int health;
    public float movespeed;
    public float timer;

}
