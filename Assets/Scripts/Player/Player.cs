using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{

    private int moneyAmount;
    private int health;

    private Tower SelectedTower;

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
    }


    // Update is called once per frame
    void Update()
    { 

        if (Input.GetMouseButtonDown(0))
            Events.ExpandFog();

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
    public void SetHealth(int amount) => health = amount;
    public Vector3 GetPosition() => transform.position;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            Events.RestartGame();
        }
        Tower tower = collision.gameObject.GetComponent<Tower>();
        if (tower != null && collision is BoxCollider2D)
        {
            if (SelectedTower != null)
                SelectedTower.ToggleShader(0);
            SelectedTower = tower;
            SelectedTower.ToggleShader(1);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Tower tower = collision.gameObject.GetComponent<Tower>();
        if (tower != null)
        {
            tower.ToggleShader(0);
            SelectedTower = null;
        }
    }
}
