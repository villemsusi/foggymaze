using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tower : MonoBehaviour
{

    public TowerData TowerData;
    
    public List<Health> EnemiesInRange;
    public List<Health> EnemiesNotVisible;
    
    private CircleCollider2D range;
    
    private Material material;

    public Image ammoDisplay;

    private float currentCooldown = 0;
    private int currentAmmo;


    private void Awake()
    {
        range = GetComponent<CircleCollider2D>();
        material = GetComponent<SpriteRenderer>().material;
    }

    private void Start()
    {
        range.radius = TowerData.ShotRadius;
        currentAmmo = TowerData.MaxAmmo;
        ToggleShader(0);

    }

    // Update is called once per frame
    void Update()
    {
        if (currentCooldown > 0)
        {
            currentCooldown -= Time.deltaTime;
        }
        else
        {
            Attack();
            currentCooldown = TowerData.ProjCooldown;
        }

        CheckNotVisibleEnemies();

    }

    private void Attack()
    {
        Health enemy = null;
        while (EnemiesInRange.Count > 0)
        {
            if (EnemiesInRange[0] == null)
                EnemiesInRange.RemoveAt(0);
            else
            {
                enemy = EnemiesInRange[0];
                break;
            }
        }
        if (enemy == null) return;
        if (currentAmmo <= 0) return;
        Shoot();
    }

    private void Shoot()
    {
        currentAmmo -= 1;
        DrawAmmoDisplay();

        transform.up = (EnemiesInRange[0].transform.position - transform.position);
        Projectile projectile = Instantiate(TowerData.ProjectilePrefab, transform.position, Quaternion.identity, transform);
        projectile.target = EnemiesInRange[0].transform;
        projectile.damage = TowerData.ProjDamage;
        projectile.speed = TowerData.ProjSpeed;
    }

    public void Reload()
    {

        if (Events.GetMoney() >= TowerData.ReloadCost && currentAmmo < TowerData.MaxAmmo)
        {
            Events.SetMoney(Events.GetMoney() - TowerData.ReloadCost);
            currentAmmo = TowerData.MaxAmmo;
            DrawAmmoDisplay();

        }
    }

    public void ToggleShader(int state)
    {
        material.SetInt("_ShowShader", state);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Health Enemy = collision.gameObject.GetComponent<Health>();
        if (Enemy != null)
        {
            if (CheckEnemyVisible(Enemy))
                EnemiesInRange.Add(Enemy);
            else
                EnemiesNotVisible.Add(Enemy);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Health Enemy = collision.gameObject.GetComponent<Health>();
        if (Enemy != null)
        {
            if (EnemiesInRange.Contains(Enemy))
                EnemiesInRange.Remove(Enemy);
            else if (EnemiesNotVisible.Contains(Enemy))
                EnemiesNotVisible.Remove(Enemy);
        }
    }

    // Function checks whether a wall is between the turret and a detected enemy
    private bool CheckEnemyVisible(Health enemy)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, enemy.transform.position - transform.position);
        foreach(RaycastHit2D hit in hits)
        {
            if (hit.collider.tag == "Enemy")
                return true;
            if (hit.collider.tag == "Wall")
                return false;
        }
        return true;
    }

    private void CheckNotVisibleEnemies()
    {
        foreach (Health enemy in EnemiesNotVisible.ToArray())
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, enemy.transform.position - transform.position);
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider.tag == "Enemy")
                {
                    EnemiesInRange.Add(enemy);
                    EnemiesNotVisible.Remove(enemy);
                    break;
                }
                if (hit.collider.tag == "Wall")
                    break;
                    
            }
        }
    }

    private void DrawAmmoDisplay()
    {
        float newAmount = (float)currentAmmo / (float)TowerData.MaxAmmo;
        Debug.Log(currentAmmo);
        Debug.Log(TowerData.MaxAmmo);
        ammoDisplay.fillAmount = newAmount;
    }
}
