using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Player : MonoBehaviour
{

    private int turretCount = 0;
    private int upgradeCount = 0;
    private int ammoCount = 0;
    private int health = 0;
    private int maxHealth;

    private Lootbox SelectedBox;
    private Tower SelectedTower;

    public Slider slider;

    private void Awake()
    {
        Events.OnGetHealth += GetHealth;
        Events.OnSetHealth += SetHealth;
        Events.OnGetAmmoCount += GetAmmo;
        Events.OnSetAmmoCount += SetAmmo;
        Events.OnSetUpgradeCount += SetUpgrade;
        Events.OnGetUpgradeCount += GetUpgrade;
        Events.OnSetTurretCount += SetTurret;
        Events.OnGetTurretCount += GetTurret;
        Events.OnGetPlayerPosition += GetPosition;
    }
    private void OnDestroy()
    {
        Events.OnGetHealth -= GetHealth;
        Events.OnSetHealth -= SetHealth;
        Events.OnGetAmmoCount -= GetAmmo;
        Events.OnSetAmmoCount -= SetAmmo;
        Events.OnSetUpgradeCount -= SetUpgrade;
        Events.OnGetUpgradeCount -= GetUpgrade;
        Events.OnSetTurretCount -= SetTurret;
        Events.OnGetTurretCount -= GetTurret;
        Events.OnGetPlayerPosition -= GetPosition;
    }

    private void Start()
    {
        SetSliderMaxHealth(health);
        maxHealth = health;
    }


    // Update is called once per frame
    void Update()
    { 

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (SelectedTower != null)
            {
                SelectedTower.Reload();
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (SelectedTower != null)
            {
                SelectedTower.Upgrade();
            }
            if (SelectedBox != null)
            {
                SelectedBox.Open();
            }
        }
    }

    // Get / Set
    public int GetUpgrade() => upgradeCount;
    public void SetUpgrade(int amount) => upgradeCount = amount;
    public int GetTurret() => turretCount;
    public void SetTurret(int amount) => turretCount = amount;
    public int GetAmmo() => ammoCount;
    public void SetAmmo(int amount) => ammoCount = amount;
    public int GetHealth() => health;
    public void SetHealth(int amount)
    {
        health = Mathf.Clamp(amount, 0, maxHealth);
        slider.value = health;
        if (health <= 0)
            Events.RestartGame();
    }
    public Vector3 GetPosition() => transform.position;

    void SetSliderMaxHealth(int amount) => slider.maxValue = amount;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Tower tower = collision.gameObject.GetComponent<Tower>();
        Lootbox lootbox = collision.gameObject.GetComponent<Lootbox>();
        if (tower != null && collision is BoxCollider2D)
        {
            if (SelectedTower != null)
                ToggleSelectionShader(0, SelectedTower.gameObject);
            SelectedTower = tower;
            ToggleSelectionShader(1, SelectedTower.gameObject);
        }
        if (lootbox != null)
        {
            if (SelectedTower != null)
                ToggleSelectionShader(0, SelectedBox.gameObject);
            SelectedBox = lootbox;
            ToggleSelectionShader(1, SelectedBox.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Tower tower = collision.gameObject.GetComponent<Tower>();
        if (tower != null)
        {
            ToggleSelectionShader(0, tower.gameObject);
            SelectedTower = null;
        }
        Lootbox lootbox = collision.gameObject.GetComponent<Lootbox>();
        if (lootbox != null)
        {
            ToggleSelectionShader(0, lootbox.gameObject);
            SelectedBox = null;
        }
    }


    private void ToggleSelectionShader(int state, GameObject obj)
    {
        obj.GetComponent<SpriteRenderer>().material.SetInt("_ShowShader", state);
    }
}
