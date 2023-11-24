using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public float smoothSpeed = 0.09f;
    private float z;

    private void Start()
    {
        z = transform.position.z;
        Camera cam = Camera.main;
    }

    private void FixedUpdate()
    {

        Vector3 smoothedPos = Vector3.Lerp(transform.position, Events.GetPlayerPosition(), smoothSpeed);
        smoothedPos.z = z;
        transform.position = smoothedPos;
    }
}
