using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {
            switch(transform.tag)
            {
                case "TurretItem":
                    Events.SetTurretCount(Events.GetTurretCount() + 1);
                    Debug.Log(Events.GetTurretCount());
                    break;
                case "UpgradeItem":
                    Events.SetUpgradeCount(Events.GetUpgradeCount() + 1);
                    Debug.Log(Events.GetUpgradeCount());
                    break;
                case "AmmoItem":
                    Events.SetAmmoCount(Events.GetAmmoCount() + 1);
                    Debug.Log(Events.GetAmmoCount());
                    break;
                case "HealthOrb":
                    Events.SetHealth(Events.GetHealth() + 20);
                    break;
                default:
                    return;

            }
            Destroy(gameObject);
        }
    }
}
