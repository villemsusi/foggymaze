using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourcePool : MonoBehaviour
{
    public static AudioSourcePool Instance;

    public AudioSource AudioSourcePrefab;

    private List<AudioSource> audioSources;

    public void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }


        Instance = this;
        audioSources = new List<AudioSource>();
        DontDestroyOnLoad(gameObject);


    }

    public AudioSource GetSource()
    {
        foreach (AudioSource source in audioSources)
        {
            if (!source.isPlaying)
            {
                return source;
            }
        }
        AudioSource newSource = Instantiate(AudioSourcePrefab, transform);
        audioSources.Add(newSource);
        return newSource;
    }


}
