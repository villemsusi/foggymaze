using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

    private int healthPoints;

    public void SetHealth(int amount) => healthPoints = amount;
    public void Damage(int damageAmount)
    {
        healthPoints -= damageAmount;
        if (healthPoints <= 0)
        {
            Destroy(gameObject);
        }
    }
}
