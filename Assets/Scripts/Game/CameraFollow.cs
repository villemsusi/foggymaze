using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public float smoothSpeed = 0.09f;
    private float z;
    private float height;
    private float width;
    GameObject Fog;

    private void Start()
    {
        z = transform.position.z;
        Camera cam = Camera.main;
        height = cam.orthographicSize * 2f + 0.5f;
        width = height * cam.aspect + 0.5f;
        //Fog = transform.GetChild(0).gameObject;
        //newScale(Fog, height, width);
    }

    private void FixedUpdate()
    {

        Vector3 smoothedPos = Vector3.Lerp(transform.position, Events.GetPlayerPosition(), smoothSpeed);
        smoothedPos.z = z;
        transform.position = smoothedPos;
        //newScale(Fog, height, width);
    }

    public void newScale(GameObject obj, float newY, float newX)
    {

        float h = obj.GetComponent<Renderer>().bounds.size.y;
        float w = obj.GetComponent<Renderer>().bounds.size.x;

        Vector3 rescale = obj.transform.localScale;

        rescale.y = newY * rescale.y / h;
        rescale.x = newX * rescale.x / w;

        obj.transform.localScale = rescale;

    }

}
