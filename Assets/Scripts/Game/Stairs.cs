using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stairs : MonoBehaviour
{

    private ParticleSystem ps;
    private GameObject trapdoor;
    public Sprite OpenTrapdoor;

    private bool isOpen;

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
        ps.Stop();

        trapdoor = transform.GetChild(0).gameObject;

        isOpen = false;

        Events.OnEnableStairs += StartFog;
        Events.OnGetStairsOpen += GetStairsOpen;
    }

    private void OnDestroy()
    {
        Events.OnEnableStairs -= StartFog;
        Events.OnGetStairsOpen -= GetStairsOpen;
    }

    private void StartFog()
    {
        if (!ps.isPlaying)
        {
            ps.Play();
            trapdoor.GetComponent<SpriteRenderer>().sprite = OpenTrapdoor;
            isOpen = true;
        }
            
    }
    private bool GetStairsOpen() => isOpen;
}
