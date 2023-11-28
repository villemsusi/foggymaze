using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private float trauma;
    private GameObject parent;
    public float MaxOffset = 0.2f;
    public float MaxAngle = 2f;

    private void Awake()
    {
        Events.OnSetTrauma += SetTrauma;
        Events.OnGetTrauma += GetTrauma;

        parent = transform.parent.gameObject;
    }

    private void OnDestroy()
    {
        Events.OnSetTrauma -= SetTrauma;
        Events.OnGetTrauma -= GetTrauma;
    }

    private void Update()
    {
        if (trauma > 0)
        {
            float x = Random.Range(-1f, 1f) * trauma * trauma * MaxOffset;
            float y = Random.Range(-1f, 1f) * trauma * trauma * MaxOffset;
            float angle = Random.Range(-1f, 1f) * trauma * trauma * MaxAngle;
            transform.localPosition = new Vector3(x, y, transform.localPosition.z);
            transform.localEulerAngles = new Vector3(0, 0, angle);
            trauma -= Time.deltaTime;
        }
        else if (trauma < 0)
        {
            trauma = 0;
            transform.localPosition = Vector3.zero;
            transform.localEulerAngles = Vector3.zero;
        }
            
        
    }

    void SetTrauma(float amount)
    {
        trauma = Mathf.Clamp(amount, 0f, 1f);
    }

    float GetTrauma() => trauma;
}
