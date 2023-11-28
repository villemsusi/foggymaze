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
        DataManager.Instance.LevelUpAudio.Play();
        AugmentSelector = GameObject.Find("Augments");
        AugmentSelector.SetActive(false);

        Events.SetTurretCount(Events.GetStartingTurretCount());
        Events.SetAmmoCount(0);
        Events.SetUpgradeCount(0);
        timer = Events.GetStageTimer();

        Events.SetTimer((int)timer);
    }


    private void FixedUpdate()
    {
        timer -= Time.deltaTime;
    
        if (Mathf.RoundToInt(timer) <= 0)
        {
            Events.EnableStairs();
        }
        if (timer >= 0)
        {
            Events.SetTimer((int)Mathf.Round(timer));
        }
    }



    private void NextStage()
    {
        if (Stage.SceneName == "MainStage")
        {
            Events.SetLevelProgress(Events.GetLevelProgress() + 1);
            SceneManager.LoadScene(Stage.SceneName);
        }
        else if (Stage.NextSceneName != "")
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
