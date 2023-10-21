using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FoggyMaze/Stage")]
public class StageData : ScriptableObject
{
    public string PresentedName;
    public string SceneName;
    public int LevelNr;

    public string NextSceneName;

    public int LootboxCount;
    public int TurretDropCount;

    public int StartingTurretCount;
    public float Timer;
    
    //public List<(Enemy, int)> Enemies;
    

}
