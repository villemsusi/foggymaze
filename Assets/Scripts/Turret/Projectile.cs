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
            target.GetComponent<Health>().Damage(damage, transform.up.normalized);
            Destroy(gameObject);
        }
    }
}
