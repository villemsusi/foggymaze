using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{

    private int moneyAmount;
    private int health;

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


    // Update is called once per frame
    void Update()
    { 

        if (Input.GetMouseButtonDown(0))
            Events.ExpandFog();
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
    }
}
