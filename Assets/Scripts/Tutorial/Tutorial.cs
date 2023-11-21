using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{
    
    public Item UpgradePrefab;
    public Item HealthOrbPrefab;
    public Item AmmoPrefab;

    public LootboxTutorial LootboxPrefab;
    public Lootbox LootboxPrefab2;

    public Enemy EnemyPrefab;

    public Canvas End;
    public TextMeshProUGUI Timer;

    public TextMeshProUGUI TutorialText;

    private int triggerCounter = 0;

    float timer = 15f;

    bool initialEnemiesSpawned = false;
    bool endStarted = false;

    private void Awake()
    {
        Events.OnNextStage += NextStage;
        Events.OnAugmentsEnable += EndTutorial;
    }
    private void OnDestroy()
    {
        Events.OnNextStage -= NextStage;
        Events.OnAugmentsEnable -= EndTutorial;
    }

    void Start()
    {
        Events.SetHealthPerm(100000);
        Events.SetHealth(100000);

        LootboxTutorial ammoDrop = Instantiate(LootboxPrefab, new Vector3(-7.5f, -13.8f, 0), Quaternion.identity, null);
        Events.AddInteractable(ammoDrop.gameObject);
        ammoDrop.SetSelectedItem(AmmoPrefab);

        LootboxTutorial upgradeDrop = Instantiate(LootboxPrefab, new Vector3(-5.5f, -13.8f, 0), Quaternion.identity, null);
        Events.AddInteractable(upgradeDrop.gameObject);
        upgradeDrop.SetSelectedItem(UpgradePrefab);

        LootboxTutorial healthDrop = Instantiate(LootboxPrefab, new Vector3(-9.5f, -13.8f, 0), Quaternion.identity, null);
        Events.AddInteractable(healthDrop.gameObject);
        healthDrop.SetSelectedItem(HealthOrbPrefab);

        Lootbox box1 = Instantiate(LootboxPrefab2, new Vector3(-1.5f, -8.8f, 0), Quaternion.identity, null);
        Events.AddInteractable(box1.gameObject);

        Lootbox box2 = Instantiate(LootboxPrefab2, new Vector3(10.5f, -7.8f, 0), Quaternion.identity, null);
        Events.AddInteractable(box2.gameObject);

        Lootbox box3 = Instantiate(LootboxPrefab2, new Vector3(-1.5f, -15.8f, 0), Quaternion.identity, null);
        Events.AddInteractable(box3.gameObject);

        Lootbox box4 = Instantiate(LootboxPrefab2, new Vector3(8.5f, -16.8f, 0), Quaternion.identity, null);
        Events.AddInteractable(box4.gameObject);

        Events.SetUpgradeCount(5);
    }


    void Update()
    {
        
        switch (triggerCounter)
        {
            case 1:
                TutorialText.text = "You can see the amount of items you have in the upper left corner";
                break;
            case 2:
                TutorialText.text = "Step on a turret to pick it up";
                break;
            case 3:
                TutorialText.text = "Press E near a barrel to open it";
                break;
            case 4:
                TutorialText.text = "Press E near a barrel to open it\nInstant Health";
                break;
            case 5:
                TutorialText.text = "Press E near a barrel to open it\nReload for turret";
                break;
            case 6:
                TutorialText.text = "Press E near a barrel to open it\nUpgrade for turret";
                break;
            case 7:
                TutorialText.text = "Press Q to open turret building view\nPress E to upgrade turret\nPress R to reload turret";
                break;
            case 8:
                TutorialText.text = "Kill the enemies";
                if (!initialEnemiesSpawned)
                {
                    Instantiate(EnemyPrefab, new(7.5f, -9.5f, 0), Quaternion.identity, null);
                    Instantiate(EnemyPrefab, new(0.5f, -6.5f, 0), Quaternion.identity, null);
                    Instantiate(EnemyPrefab, new(10.5f, -3.5f, 0), Quaternion.identity, null);
                    initialEnemiesSpawned = true;
                }
                if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
                {
                    TutorialText.text = "";
                }
                break;
            case 9:
                if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0 && !endStarted)
                {
                    TutorialText.text = "Survive for the duration of the timer";
                    Invoke(nameof(SpawnEnemy), 0.5f);
                    Timer.gameObject.SetActive(true);
                    Events.SetTimer((int)timer);
                    endStarted = true;
                }
                if (endStarted)
                {
                    timer -= Time.deltaTime;
                    Events.SetTimer(Mathf.Max(0, (int)timer));
                }
                break;

        }
        if (timer <= 0)
        {
            TutorialText.text = "Find the stairs and exit by pressing E";
            Events.EnableStairs();
        }
            
    }


    void SpawnEnemy()
    {
        Instantiate(EnemyPrefab, new(7.5f, -9.5f, 0), Quaternion.identity, null);
        Invoke(nameof(SpawnEnemy), 2);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Trigger"))
        {
            triggerCounter += 1;
            Destroy(collision.gameObject);
        }
    }


    void EndTutorial()
    {
        End.gameObject.SetActive(true);
    }

    public void NextStage()
    {
        DataManager.Instance.SetInitialStats();
        SceneManager.LoadScene("MainStage");
    }
}
