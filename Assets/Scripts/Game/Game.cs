using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    

    private void Awake()
    {
        Events.OnRestartGame += RestartGame;

        Events.SetTurretCount(3);
        Events.SetAmmoCount(0);
        Events.SetUpgradeCount(0);
        Events.SetHealth(100);

    }
    private void OnDestroy()
    {
        Events.OnRestartGame -= RestartGame;
        
    }

    private void Start()
    {
        
    }


    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
