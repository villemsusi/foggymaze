using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{

    public TowerData TowerData;
    public List<Health> EnemiesInRange;
    public List<Health> EnemiesNotVisible;
    private CircleCollider2D range;


    private float currentCooldown = 0;


    private void Awake()
    {
        range = GetComponent<CircleCollider2D>();
    }

    private void Start()
    {
        range.radius = TowerData.ShotRadius;
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

        Shoot();
    }

    private void Shoot()
    {
        transform.up = (EnemiesInRange[0].transform.position - transform.position);
        Projectile projectile = Instantiate(TowerData.ProjectilePrefab, transform.position, Quaternion.identity, transform);
        projectile.target = EnemiesInRange[0].transform;
        projectile.damage = TowerData.ProjDamage;
        projectile.speed = TowerData.ProjSpeed;
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
}
