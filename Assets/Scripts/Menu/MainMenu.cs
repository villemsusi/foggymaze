using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void PlayGame()
    {
        Time.timeScale = 1;
        DataManager.Instance.SetInitialStats();
        SceneManager.LoadScene("MainStage");
    }
   
    public void PlayTutorial()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("TutorialStage");
    }

    public void GoMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
