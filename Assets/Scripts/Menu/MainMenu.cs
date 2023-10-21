using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    private float movespeed = 4f;
    private int health = 100;

    private void Awake()
    {
        Events.SetHealthPerm(health);
        Events.SetMovespeedPerm(movespeed);
    }


    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
   
    public void QuitGame()
    {
        Application.Quit();
    }
}
