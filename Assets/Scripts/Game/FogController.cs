using System.Collections;
using System.Collections.Generic;
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
        //if (Input.GetKeyDown(KeyCode.Space))
        //    SetFogScale(fogScale * 0.9f);
    }


    void SetFogScale(float scale)
    {
        fogScale = scale;
        mat.SetFloat("_Scale", scale);
    }
    float GetFogScale() => fogScale;

}
