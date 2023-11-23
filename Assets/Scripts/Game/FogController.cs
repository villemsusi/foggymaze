using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FogController : MonoBehaviour
{
    private float fogScale;

    private Material mat;

    private void Awake()
    {
        Events.OnSetFogScale += SetFogScale;
        Events.OnGetFogScale += GetFogScale;

        mat = GetComponent<SpriteRenderer>().material;
    }

    private void OnDestroy()
    {
        Events.OnSetFogScale -= SetFogScale;
        Events.OnGetFogScale -= GetFogScale;
    }


    void Start()
    {
        SetFogScale(Events.GetFogScalePerm());
    }



    private void Update()
    {
        float width = (float)(Camera.main.orthographicSize * 2.0 * Screen.width / Screen.height);
        float height = width * Screen.height / Screen.width;
        transform.localScale = new Vector3(width, height, 1);
    }


    void SetFogScale(float scale)
    {
        fogScale = scale;
        mat.SetFloat("_Scale", scale);
    }
    float GetFogScale() => fogScale;

}
