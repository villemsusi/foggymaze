using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void PlayGame()
    {
        DataManager.Instance.SetInitialStats();
        SceneManager.LoadScene("Stage1");
    }
   


    public void QuitGame()
    {
        Application.Quit();
    }
}
