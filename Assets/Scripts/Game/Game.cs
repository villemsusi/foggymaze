using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    

    private void Awake()
    {
        Events.OnRestartGame += RestartGame;
        
    }
    private void OnDestroy()
    {
        Events.OnRestartGame -= RestartGame;
        
    }


    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
