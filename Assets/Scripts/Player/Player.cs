using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Player : MonoBehaviour
{

    private int moneyAmount;
    private int health;

    private Tower SelectedTower;
    public Slider slider;

    private void Awake()
    {
        Events.OnGetHealth += GetHealth;
        Events.OnSetHealth += SetHealth;
        Events.OnGetMoney += GetMoney;
        Events.OnSetMoney += SetMoney;
        Events.OnGetPlayerPosition += GetPosition;
    }
    private void OnDestroy()
    {
        Events.OnGetHealth -= GetHealth;
        Events.OnSetHealth -= SetHealth;
        Events.OnGetMoney -= GetMoney;
        Events.OnSetMoney -= SetMoney;
        Events.OnGetPlayerPosition -= GetPosition;
    }

    private void Start()
    {
        Events.SetMoney(50);
        Events.SetHealth(100);
        SetSliderMaxHealth(health);
    }


    // Update is called once per frame
    void Update()
    { 

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (SelectedTower != null)
            {
                SelectedTower.Reload();
                Debug.Log(Events.GetMoney().ToString());
            }
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            if (SelectedTower != null)
            {
                SelectedTower.Upgrade();
            }
        }
    }

    public int GetMoney() => moneyAmount;
    public void SetMoney(int amount) => moneyAmount = amount;
    public int GetHealth() => health;
    public void SetHealth(int amount)
    {
        health = amount;
        slider.value = health;
        if (health <= 0)
            Events.RestartGame();
    }
    public Vector3 GetPosition() => transform.position;

    void SetSliderMaxHealth(int amount) => slider.maxValue = amount;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Tower tower = collision.gameObject.GetComponent<Tower>();
        if (tower != null && collision is BoxCollider2D)
        {
            if (SelectedTower != null)
                SelectedTower.ToggleSelectionShader(0);
            SelectedTower = tower;
            SelectedTower.ToggleSelectionShader(1);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Tower tower = collision.gameObject.GetComponent<Tower>();
        if (tower != null)
        {
            tower.ToggleSelectionShader(0);
            SelectedTower = null;
        }
    }
}
