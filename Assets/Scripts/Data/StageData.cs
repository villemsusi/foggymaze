using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FoggyMaze/Stage")]
public class StageData : ScriptableObject
{
    public string PresentedName;
    public string SceneName;
    public int LevelNr;

    public float Timer;
    public int StartingTurretCount;
    public List<Enemy> Enemies;
    

}
