using System;
using UnityEngine;

public class Shield : MonoBehaviour
{
    

    void Update()
    {
        
        Vector2 direction = transform.parent.transform.parent.GetComponent<Enemy>().MoveInput;

        transform.parent.transform.up = Vector3.Lerp(transform.parent.transform.up, direction, Time.deltaTime*4);
        

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Projectile"))
        {
            DataManager.Instance.ShieldBlockAudio.Play();
            Destroy(other.gameObject);
        }
    }



}
