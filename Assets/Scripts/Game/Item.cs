using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{

    private float t;

    private Vector3 spawnPos;
    private Vector3 upperPos;
    private Vector3 landPos;

    private float xOffset;

    SpriteRenderer rend;

    private void Start()
    {
        rend = GetComponent<SpriteRenderer>();

        t = 0;

        xOffset = Random.Range(-0.5f, 0.5f);

        spawnPos = transform.position;
        upperPos = spawnPos + new Vector3(xOffset, 3, 0);
        landPos = spawnPos + new Vector3(xOffset, 0, 0);

        if (!transform.CompareTag("TurretItem"))
            StartCoroutine(DropCurve());
    }

    private IEnumerator DropCurve()
    {

        int rotation = Random.Range(540, 900);
        while (t < 1)
        {

            transform.position = Mathf.Pow(1 - t, 2) * spawnPos +
                2 * (1 - t) * t * upperPos +
                Mathf.Pow(t, 2) * landPos;

            transform.rotation = Quaternion.Euler(0, 0, t * rotation);

            var col = rend.color;
            col.a = 1 / t - 0.7f;
            rend.color = col;

            yield return new WaitForEndOfFrame();

            t += Mathf.Lerp(0, 1, 2f * Time.deltaTime * Mathf.Max(0.7f, (1 - Mathf.Sin(Mathf.PI * t))));
        }

        t = 0f;

        switch (transform.tag)
        {
            case "UpgradeItem":
                Events.SetUpgradeCount(Events.GetUpgradeCount() + 1);
                break;
            case "AmmoItem":
                Events.SetAmmoCount(Events.GetAmmoCount() + 1);
                break;
            case "HealthOrb":
                Events.SetHealth(Events.GetHealth() + 65);
                break;
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null)
            if (transform.tag == "TurretItem")
            {
                DataManager.Instance.TurretPickupAudio.Play();
                Events.SetTurretCount(Events.GetTurretCount() + 1);
                Destroy(gameObject);
            }
                

    }
}
