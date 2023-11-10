using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAudio : MonoBehaviour
{
    public AudioClipGroup WaterDrop;

    private List<AudioClipGroup> ambientSounds;

    // Start is called before the first frame update
    void Start()
    {
        ambientSounds = new List<AudioClipGroup>();
        ambientSounds.Add(WaterDrop);

        Invoke(nameof(PlaySound), Random.Range(3, 7));
    }


    void PlaySound()
    {
        // Selects a random Vector which has either -1 or 1 as x and y
        Vector3 offset = new(Random.Range(0, 2)*2-1, Random.Range(0, 2)*2-1, 0);
        
        // Selects random sound from list of sounds and plays it
        int sound = Random.Range(0, ambientSounds.Count);
        ambientSounds[sound].Play(Events.GetPlayerPosition() + offset);

        // Call this function again at a random time interval
        Invoke(nameof(PlaySound), Random.Range(3, 7));
    }
}
