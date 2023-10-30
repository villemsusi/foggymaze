using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;


public class Player : MonoBehaviour
{

    private int turretCount = 0;
    private int upgradeCount = 0;
    private int ammoCount = 0;
    private int health;
    private float movespeed;

    private bool onStairs;

    private GameObject SelectedItem;
    private static readonly float selectionRadius = 0.7f;

    public List<GameObject> Interactables;

    public Slider slider;
    public TurretBuilder turretBuilder;

    private Light2D aura;

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
        Events.OnGetIsItemSelected += IsItemSelected;
        Events.OnAddInteractable += AddInteractable;
        Events.OnRemoveInteractable += RemoveInteractable;


        aura = transform.Find("AuraLight").GetComponent<Light2D>();

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
        Events.OnGetIsItemSelected -= IsItemSelected;
        Events.OnAddInteractable -= AddInteractable;
        Events.OnRemoveInteractable -= RemoveInteractable;
    }


    private void Start()
    {
        SetSliderMaxHealth(Events.GetHealthPerm());
        SetHealth(Events.GetHealthPerm());

        onStairs = false;

        aura.color = Events.GetAuraColor();
        aura.intensity = 7f;
        aura.pointLightOuterRadius = 3;
    }


    // Update is called once per frame
    void Update()
    {
        SelectInteractable();
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (SelectedItem != null)
            {
                if (SelectedItem.GetComponent<Turret>() != null)
                    SelectedItem.GetComponent<Turret>().Reload();
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

            if (SelectedItem != null)
            {
                if (SelectedItem.GetComponent<Turret>() != null)
                    SelectedItem.GetComponent<Turret>().Upgrade();
                else if (SelectedItem.GetComponent<Lootbox>() != null)
                    SelectedItem.GetComponent<Lootbox>().Open();
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


    public int GetHealth() => health;
    public void SetHealth(int amount)
    {
        health = Mathf.Clamp(amount, 0, Events.GetHealthPerm());
        slider.value = health;
        if (health <= 0)
            Events.RestartGame();
    }


    public Vector3 GetPosition() => transform.position;
    public bool IsItemSelected() => SelectedItem != null;

    void SetSliderMaxHealth(int amount) => slider.maxValue = amount;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Stairs stairs = collision.gameObject.GetComponent<Stairs>();
        if (stairs != null)
        {
            onStairs = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Stairs stairs = collision.gameObject.GetComponent<Stairs>();
        if (stairs != null)
        {
            onStairs = false;
        }
    }

    private void SelectInteractable()
    {
        if (SelectedItem != null) 
        { 
            if ((SelectedItem.transform.position - transform.position).sqrMagnitude > Mathf.Pow(selectionRadius, 2))
            {
                ToggleSelectionShader(0, SelectedItem);
                SelectedItem = null;
                return;
            }
        }
        Transform t = GetClosestInteractable(Interactables);
        if (t != null)
        {
            if (SelectedItem != null && SelectedItem != t.gameObject)
            {
                ToggleSelectionShader(0, SelectedItem);
            }
            SelectedItem = t.gameObject;
            ToggleSelectionShader(1, SelectedItem);
            return;
        }
            
    }
    private void AddInteractable(GameObject inter)
    {
        if (!Interactables.Contains(inter))
            Interactables.Add(inter);
    }
    private void RemoveInteractable(GameObject inter)
    {
        if (Interactables.Contains(inter))
            Interactables.Remove(inter);
    }

    private Transform GetClosestInteractable(List<GameObject> interactables)
    {
        Transform tMin = null;
        float minDist = selectionRadius;
        minDist = Mathf.Pow(minDist, 2);
        Vector3 currentPos = transform.position;
        foreach (GameObject t in interactables)
        {
            Vector3 direction = t.transform.position - currentPos;
            float dist = direction.sqrMagnitude;
            if (dist < minDist)
            {
                tMin = t.transform;
                minDist = dist;
            }
        }
        return tMin;
    }


    private void ToggleSelectionShader(int state, GameObject obj)
    {
        obj.GetComponent<SpriteRenderer>().material.SetInt("_ShowShader", state);
    }
}
