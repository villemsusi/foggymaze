using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private bool coroutineAllowed;
    private bool canPickup;
    private float t;
    private float speed;

    private Vector3 spawnPos;
    private Vector3 upperPos;
    private Vector3 landPos;

    private float xOffset;


    private void Start()
    {
        coroutineAllowed = true;
        if (transform.CompareTag("TurretItem"))
            canPickup = true;
        else
            canPickup = false;
        t = 0;

        xOffset = Random.Range(-0.5f, 0.5f);

        spawnPos = transform.position;
        upperPos = spawnPos + new Vector3(xOffset, 2, 0);
        landPos = spawnPos + new Vector3(xOffset, 0, 0);
    }

    private void Update()
    {
        if (coroutineAllowed && !transform.CompareTag("TurretItem"))
            StartCoroutine(DropCurve());
    }

    private IEnumerator DropCurve()
    {
        coroutineAllowed = false;

        while (t < 1)
        {
            speed = 1 - Mathf.Sin(Mathf.PI * t);

            transform.position = Mathf.Pow(1 - t, 2) * spawnPos +
                2 * (1 - t) * t * upperPos +
                Mathf.Pow(t, 2) * landPos;
            transform.rotation = Quaternion.Euler(0, 0, t * 1440);
            yield return new WaitForEndOfFrame();
            if (speed == 0)
            {
                t += Time.deltaTime * 0.1f;
                speed = 0.1f;
            }
            else
                t += Mathf.Lerp(0, 1, Time.deltaTime);
        }

        t = 0f;
        canPickup = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null && canPickup)
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
