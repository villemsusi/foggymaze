using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

    private int healthPoints;

    private ParticleSystem ps;

    private void Awake()
    {
        ps = transform.Find("HitParticle").GetComponent<ParticleSystem>();
    }

    public void SetHealth(int amount) => healthPoints = amount;

    public void Damage(int damageAmount, Vector3 direction)
    {
        healthPoints -= damageAmount;
        direction = direction.normalized;
        float deg = Mathf.Atan2(direction.y, direction.x) * 180 / Mathf.PI;
        
        ps.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, deg));
        ps.Play();
        
        

        transform.GetComponent<SpriteRenderer>().color = Color.white;
        if (healthPoints <= 0)
        {
            Destroy(gameObject);
        }
    }
}
