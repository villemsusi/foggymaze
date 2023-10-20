using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    public StageData Stage;

    public Canvas AugmentSelector;

    private float timer;
    private void Awake()
    {
        Time.timeScale = 1;

        Events.OnAugmentsEnable += EnableAugments;
        Events.OnRestartGame += RestartGame;
        Events.OnNextStage += NextStage;

        Events.SetTurretCount(Stage.StartingTurretCount);
        Events.SetAmmoCount(0);
        Events.SetUpgradeCount(0);
        Events.SetHealth(Events.GetHealthPerm());
        Events.SetMovespeed(Events.GetMovespeedPerm());

        AugmentSelector.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        Events.OnAugmentsEnable -= EnableAugments;
        Events.OnRestartGame -= RestartGame;
        Events.OnNextStage -= NextStage;

    }

    private void Start()
    {
        timer = Stage.Timer;

        Debug.Log("Turrets: " + Events.GetTurretCount());
        Debug.Log("Upgrades: " + Events.GetUpgradeCount());
        Debug.Log("Ammo: " + Events.GetAmmoCount());
    }


    private void FixedUpdate()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Events.EnableStairs();
        }
    }


    private void NextStage()
    {
        SceneManager.LoadScene(Stage.NextSceneName);
    }

    private void EnableAugments()
    {
        AugmentSelector.gameObject.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
