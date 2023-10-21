using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public StageData Stage;

    public GameObject AugmentSelector;

    private float timer;
    private void Awake()
    {
        Time.timeScale = 1;

        Events.OnAugmentsEnable += EnableAugments;
        Events.OnRestartGame += RestartGame;
        Events.OnNextStage += NextStage;
        Events.OnGetLootboxCount += GetLootboxCount;
        Events.OnGetTurretDropCount += GetTurretDropCount;


        Events.SetTurretCount(Stage.StartingTurretCount);
        Events.SetAmmoCount(0);
        Events.SetUpgradeCount(0);
        Events.SetHealth(Events.GetHealthPerm());
        Events.SetMovespeed(Events.GetMovespeedPerm());

        AugmentSelector.SetActive(false);
    }

    private void OnDestroy()
    {
        Events.OnAugmentsEnable -= EnableAugments;
        Events.OnRestartGame -= RestartGame;
        Events.OnNextStage -= NextStage;
        Events.OnGetLootboxCount -= GetLootboxCount;
        Events.OnGetTurretDropCount -= GetTurretDropCount;

    }

    private void Start()
    {
        timer = Stage.Timer;

    }


    private void FixedUpdate()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Events.EnableStairs();
        }
    }

    

    private int GetLootboxCount() => Stage.LootboxCount;

    private int GetTurretDropCount() => Stage.TurretDropCount;


    private void NextStage()
    {
        if (Stage.NextSceneName != "")
            SceneManager.LoadScene(Stage.NextSceneName);
        else
            SceneManager.LoadScene("Menu");
    }

    private void EnableAugments()
    {
        AugmentSelector.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

}
