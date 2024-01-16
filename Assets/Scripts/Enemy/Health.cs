using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

    private int healthPoints;

    private SpriteRenderer srenderer;

    private ParticleSystem ps;

    private void Awake()
    {
        ps = transform.Find("HitParticle").GetComponent<ParticleSystem>();
        srenderer = GetComponent<SpriteRenderer>();
    }

    public void SetHealth(int amount)
    {
        healthPoints = amount;
        if (amount < GetComponent<Enemy>().EnemyData.Health)
            srenderer.color -= new Color(0,0,0,1 - (float)amount / (float)GetComponent<Enemy>().EnemyData.Health);
        Debug.Log(srenderer.color.a);
    }

    public void Damage(int damageAmount, Vector3 direction)
    {
        direction = direction.normalized;

        SetHealth(healthPoints - damageAmount);
        if (ps != null)
        {
            float deg = Mathf.Atan2(direction.y, direction.x) * 180 / Mathf.PI;
            ps.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, deg));
            ps.Play();
        }
        //transform.position += direction * 0.1f;
        gameObject.GetComponent<Enemy>().SetForce(direction*2);
        
        

        transform.GetComponent<SpriteRenderer>().color = Color.white;
        if (healthPoints <= 0)
        {
            DataManager.Instance.EnemyDeathAudio.Play();
            Destroy(gameObject);
        }
    }
    
}
