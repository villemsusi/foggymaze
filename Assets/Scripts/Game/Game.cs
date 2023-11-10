using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    public StageData Stage;

    private GameObject AugmentSelector;

    private float timer;
    private void Awake()
    {
        Time.timeScale = 1;

        Events.OnAugmentsEnable += EnableAugments;
        Events.OnRestartGame += RestartGame;
        Events.OnNextStage += NextStage;

    }

    private void OnDestroy()
    {
        Events.OnAugmentsEnable -= EnableAugments;
        Events.OnRestartGame -= RestartGame;
        Events.OnNextStage -= NextStage;

    }

    private void Start()
    {
        AugmentSelector = GameObject.Find("Augments");
        AugmentSelector.SetActive(false);

        Events.SetTurretCount(Events.GetStartingTurretCount());
        Events.SetAmmoCount(50);
        Events.SetUpgradeCount(50);
        timer = Events.GetStageTimer();

        Events.SetTimer((int)timer);

        Debug.Log("Level - " + Events.GetLevelProgress());

    }


    private void FixedUpdate()
    {
        timer -= Time.deltaTime;
    
        if (timer <= 0)
        {
            Events.EnableStairs();
        }
        if (timer > 0)
        {
            Events.SetTimer((int)Mathf.Round(timer));
        }
    }



    private void NextStage()
    {
        
        if (Stage.NextSceneName != "")
        {
            Events.SetLevelProgress(Events.GetLevelProgress() + 1);
            SceneManager.LoadScene(Stage.NextSceneName);
        }
            
        else
            SceneManager.LoadScene("Menu");
    }

    private void EnableAugments()
    {
        AugmentSelector.SetActive(true);
    }

    public void RestartGame()
    { 
        SceneManager.LoadScene("GameOver");
    }

}
