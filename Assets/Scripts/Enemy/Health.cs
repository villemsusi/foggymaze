using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

    public int healthPoints;

    public void Damage(int damageAmount)
    {
        healthPoints -= damageAmount;
        if (healthPoints <= 0)
        {
            Events.SetMoney(Events.GetMoney() + 2);
            Destroy(gameObject);
        }
    }
}
