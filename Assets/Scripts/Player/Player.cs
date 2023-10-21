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
    private float movespeed;

    private bool onStairs;

    private Lootbox SelectedBox;
    private Turret SelectedTurret;

    public Slider slider;
    public TurretBuilder turretBuilder;

    private void Awake()
    {
        Events.OnGetHealth += GetHealth;
        Events.OnSetHealth += SetHealth;
        Events.OnGetMovespeed += GetMovespeed;
        Events.OnSetMovespeed += SetMovespeed;


        Events.OnGetAmmoCount += GetAmmo;
        Events.OnSetAmmoCount += SetAmmo;
        Events.OnSetUpgradeCount += SetUpgrade;
        Events.OnGetUpgradeCount += GetUpgrade;
        Events.OnSetTurretCount += SetTurret;
        Events.OnGetTurretCount += GetTurret;


        Events.OnGetPlayerPosition += GetPosition;

        //turretBuilder.gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        Events.OnGetHealth -= GetHealth;
        Events.OnSetHealth -= SetHealth;
        Events.OnGetMovespeed -= GetMovespeed;
        Events.OnSetMovespeed -= SetMovespeed;


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
        SetSliderMaxHealth(Events.GetHealthPerm());
        SetHealth(Events.GetHealthPerm());

        onStairs = false;
    }


    // Update is called once per frame
    void Update()
    { 

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (SelectedTurret != null)
            {
                SelectedTurret.Reload();
                return;
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (Events.GetTurretCount() > 0 && !turretBuilder.gameObject.activeSelf)
            {
                turretBuilder.gameObject.SetActive(true);
                return;
            }
            else if (turretBuilder.gameObject.activeSelf)
            {
                turretBuilder.gameObject.SetActive(false);
                return;
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (onStairs && Events.GetStairsOpen())
            {
                Time.timeScale = 0;
                Events.EnableAugments();
                return;
            }

            if (SelectedTurret != null)
            {
                SelectedTurret.Upgrade();
                return;
            }


            if (SelectedBox != null)
            {
                SelectedBox.Open();
                return;
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


    public float GetMovespeed() => movespeed;
    public void SetMovespeed(float amount) => movespeed = amount;
    public int GetHealth() => health;
    public void SetHealth(int amount)
    {
        health = Mathf.Clamp(amount, 0, Events.GetHealthPerm());
        slider.value = health;
        if (health <= 0)
            Events.RestartGame();
    }


    public Vector3 GetPosition() => transform.position;

    void SetSliderMaxHealth(int amount) => slider.maxValue = amount;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        Stairs stairs = collision.gameObject.GetComponent<Stairs>();
        if (stairs != null)
        {
            onStairs = true;
        }
        Turret turret = collision.gameObject.GetComponent<Turret>();
        if (turret != null && collision is BoxCollider2D)
        {
            if (SelectedTurret != null)
                ToggleSelectionShader(0, SelectedTurret.gameObject);
            SelectedTurret = turret;
            ToggleSelectionShader(1, SelectedTurret.gameObject);
        }
        Lootbox lootbox = collision.gameObject.GetComponent<Lootbox>();
        if (lootbox != null)
        {
            if (SelectedTurret != null)
                ToggleSelectionShader(0, SelectedBox.gameObject);
            SelectedBox = lootbox;
            ToggleSelectionShader(1, SelectedBox.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Stairs stairs = collision.gameObject.GetComponent<Stairs>();
        if (stairs != null)
        {
            onStairs = false;
        }
        Turret turret = collision.gameObject.GetComponent<Turret>();
        if (turret != null)
        {
            ToggleSelectionShader(0, turret.gameObject);
            SelectedTurret = null;
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
