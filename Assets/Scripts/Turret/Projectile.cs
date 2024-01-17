using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [HideInInspector]
    public int damage;
    [HideInInspector]
    public float speed;
    [HideInInspector]
    public Transform target;

    private Material material;

    public GameObject ExplosionPrefab;


    private void Awake()
    {
        material = GetComponent<SpriteRenderer>().material;
        
    }

    private void Start()
    {
        Color col = Events.GetProjectileColor() * 2.7f;
        material.SetColor("_Color", col);

    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }
        transform.up = (target.position - transform.position);
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, target.position)<0.2f)
        {
            if (gameObject.name == "ProjectileExplosion(Clone)")
                Explosion();
            else
                Damage();
        }
    }

    public void Explosion()
    {
        Instantiate(ExplosionPrefab, transform.position, Quaternion.identity, null);
        DataManager.Instance.ExplosionAudio.Play();
        Events.SetTrauma(Events.GetTrauma() + 0.5f);

        Collider2D[] enemiesInExplosionRange = Physics2D.OverlapCircleAll(target.gameObject.transform.position, 1f);
        foreach (Collider2D col in enemiesInExplosionRange)
        {
            if (col.TryGetComponent<Health>(out var enemy))
                enemy.Damage(damage, (col.transform.position-transform.position).normalized);
        }
        Destroy(gameObject);
    }

    public void Damage()
    {
        target.GetComponent<Health>().Damage(damage, transform.up.normalized);
        Destroy(gameObject);
    }
}
