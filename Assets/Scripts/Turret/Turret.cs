using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;

public class Turret : MonoBehaviour
{

    public TurretData TurretData;
    
    public List<Health> EnemiesInRange;
    public List<Health> EnemiesNotVisible;
    
    private CircleCollider2D range;
    private AudioSource audioSource;
    private ParticleSystem ps;

    private Light2D aura;
    private Light2D barrel;

    public Image display;
    public Image display2;

    private float currentCooldown = 0;
    private int currentAmmo;


    public AudioClipGroup ShotAudio;
    public AudioClipGroup UpgradeAudio;



    private void Awake()
    {
        range = GetComponent<CircleCollider2D>();
        audioSource = GetComponent<AudioSource>();
        if (TurretData.Type == "Heal")
            ps = transform.Find("HealParticle").GetComponent<ParticleSystem>();
        else ps = null;


        aura = transform.Find("AuraLight").GetComponent<Light2D>();
        barrel = transform.Find("BarrelLight")?.GetComponent<Light2D>() ?? null;
    }

    private void Start()
    {
        range.radius = TurretData.ShotRadius;
        currentAmmo = TurretData.MaxAmmo;

        if (aura != null)
        {
            aura.color = Events.GetAuraColor();
            aura.intensity = 7f;
            aura.pointLightOuterRadius = 3;
        }
        if (barrel != null)
        {
            barrel.color = Events.GetProjectileColor();
            barrel.intensity = 7f;
            barrel.pointLightOuterRadius = 3;
        }
        
        if (display != null)
            display.color = Events.GetProjectileColor();
        if (display2 != null)
            display2.color = Events.GetProjectileColor();
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
            if (TurretData.Type == "Attack")
                Attack();
            else if (TurretData.Type == "Heal")
                Heal();
            currentCooldown = TurretData.ProjCooldown;
        }
        if (TurretData.Type == "Attack")
            CheckEnemyVisibility();

        if (TurretData.Type == "Heal")
        {
            if (ps != null)
            {
                if (Vector3.Distance(transform.position, Events.GetPlayerPosition()) <= range.radius)
                {
                    if (!ps.isPlaying)
                        ps.Play();
                }
                else
                {
                    if (ps.isPlaying)
                        ps.Stop();
                }
            }
        }

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
        DrawDisplay();

        ShotAudio.Play(transform.position);
        
        Vector3 upDir = EnemiesInRange[0].transform.position - transform.position;
        upDir.z = 0;
        transform.up = upDir;


        Recoil();

        Projectile projectile = Instantiate(TurretData.ProjectilePrefab, transform.position, Quaternion.identity, transform);
        projectile.target = EnemiesInRange[0].transform;
        projectile.damage = TurretData.ProjDamage;
        projectile.speed = TurretData.ProjSpeed;

    }
    private void Recoil()
    {
        transform.position += -transform.up * TurretData.Kickback;
        Invoke(nameof(ResetRecoil), TurretData.ProjCooldown * 0.6f);
    }
    private void ResetRecoil()
    {
        transform.position += transform.up * TurretData.Kickback;
    }

    private void Heal()
    {
        if (Vector3.Distance(transform.position, Events.GetPlayerPosition()) <= range.radius)
            Events.SetHealth(Events.GetHealth() + TurretData.ProjDamage);
    }

    public void Reload()
    {
        if (Events.GetAmmoCount() > 0 && currentAmmo < TurretData.MaxAmmo)
        {
            Events.SetAmmoCount(Events.GetAmmoCount() - 1);
            currentAmmo = TurretData.MaxAmmo;
            DrawDisplay();

        }
    }

    public void Upgrade()
    {
        if (Events.GetUpgradeCount() > 0 && TurretData.NextUpgrade != null)
        {
            UpgradeAudio.Play();

            Events.SetUpgradeCount(Events.GetUpgradeCount() - 1);
            Turret newTurret = Instantiate(TurretData.NextUpgrade, transform.position, Quaternion.identity, null);
            newTurret.transform.up = transform.up;

            Events.RemoveInteractable(gameObject);
            Events.AddInteractable(newTurret.gameObject);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (TurretData.Type == "Attack")
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
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (TurretData.Type == "Attack")
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
    }

    // Function checks whether a wall is between the turret and a detected enemy
    private bool CheckEnemyVisible(Health enemy)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, enemy.transform.position - transform.position);
        foreach(RaycastHit2D hit in hits)
        {
            if (hit.collider.CompareTag("Enemy"))
                return true;
            if (hit.collider.CompareTag("Wall"))
                return false;
        }
        return true;
    }

    // Function checks every in range enemy if they are visible
    private void CheckEnemyVisibility()
    {
        foreach (Health enemy in EnemiesNotVisible.ToArray())
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, enemy.transform.position - transform.position);
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider.CompareTag("Enemy"))
                {
                    EnemiesInRange.Add(enemy);
                    EnemiesNotVisible.Remove(enemy);
                    break;
                }
                if (hit.collider.CompareTag("Wall"))
                    break;
                    
            }
        }

        foreach (Health enemy in EnemiesInRange.ToArray())
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, enemy.transform.position - transform.position);
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider.CompareTag("Wall"))
                {
                    EnemiesNotVisible.Add(enemy);
                    EnemiesInRange.Remove(enemy);
                    break;
                }
                if (hit.collider.CompareTag("Enemy"))
                    break;

            }
        }
    }

    private void DrawDisplay()
    {
        float newAmount = (float)currentAmmo / TurretData.MaxAmmo;
        display.fillAmount = newAmount;
        if (display2 != null)
            display2.fillAmount = newAmount;
    }

}
