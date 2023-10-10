using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;

public class Game : MonoBehaviour
{
    private float fogSep;

    public static Game Instance;

    public VisualEffect vfxRenderer;
    private float fogRadius = 0.02f;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;

        fogSep = 1f;
        vfxRenderer.SetFloat("CircleRadius", fogRadius);
    }

    // Update is called once per frame
    void Update()
    {
        //if (vfxRenderer.GetFloat("CircleRadius") > fogRadius)
        //    vfxRenderer.SetFloat("CircleRadius", fogRadius*Time.deltaTime*0.3f);
        Vector3 pos = Player.Instance.transform.position;
        Debug.Log(pos);
        vfxRenderer.SetVector3("ColliderPosLeft", new Vector3(pos.x - fogSep, pos.y, pos.z));
        vfxRenderer.SetVector3("ColliderPosRight", new Vector3(pos.x + fogSep, pos.y, pos.z));
    }


    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExpandFog()
    {
        vfxRenderer.SetFloat("CircleRadius", 15f);

    }

}
