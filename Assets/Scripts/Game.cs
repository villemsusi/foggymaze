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
    private float fogRadius = 1f;
    private float fogDelay;
    private float currentRadius;
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
        FogControl();
    }



    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    // Function that controls the fogfree circle around the player
    private void FogControl()
    {
        Vector3 pos = Player.Instance.transform.position;
        vfxRenderer.SetVector3("ColliderPosLeft", new Vector3(pos.x - fogSep, pos.y, pos.z));
        vfxRenderer.SetVector3("ColliderPosRight", new Vector3(pos.x + fogSep, pos.y, pos.z));

        if (vfxRenderer.GetFloat("CircleRadius") > fogRadius && fogDelay > 0)
        {
            fogDelay -= Time.deltaTime;
            currentRadius -= 15 * Time.deltaTime;
            vfxRenderer.SetFloat("CircleRadius", currentRadius);
        }
    }

    public void ExpandFog()
    {
        vfxRenderer.SetFloat("CircleRadius", 15f);
        currentRadius = 15f;
        fogDelay = 1f;
        
    }

}
