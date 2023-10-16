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

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        transform.up = (target.position - transform.position);
        if (transform.position == target.position)
        {
            target.GetComponent<Health>().Damage(damage);
            Destroy(gameObject);
        }
    }
}
