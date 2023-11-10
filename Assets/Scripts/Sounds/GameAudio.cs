using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAudio : MonoBehaviour
{
    public AudioClipGroup WaterDrop;

    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(PlaySound), Random.Range(3, 7));
    }


    void PlaySound()
    {
        Vector3 offset = new Vector3(Random.Range(4f,5f), Random.Range(0,0), 0);
        WaterDrop.Play(Events.GetPlayerPosition() + offset);
        Invoke(nameof(PlaySound), Random.Range(3, 7));
    }
}
