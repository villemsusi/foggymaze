using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FoggyMaze/AudioClipGroup")]
public class AudioClipGroup : ScriptableObject
{
    [Range(0, 2)]
    public float VolumeMin = 1;
    [Range(0, 2)]
    public float VolumeMax = 1;
    [Range(0, 2)]
    public float PitchMin = 1;
    [Range(0, 2)]
    public float PitchMax = 1;

    public bool Loop;

    public float Cooldown = 0.1f;

    public List<AudioClip> Clips;


    private float timestamp;
    private int lastClip;
    private int currClip;

    private void OnEnable()
    {
        timestamp = 0;
        lastClip = 0;
    }

    public void Play()
    {
        Debug.Log("PLAY");
        if (AudioSourcePool.Instance == null) return;
        if (Clips.Count <= 0) return;
        Play(AudioSourcePool.Instance.GetSource());
        

    }

    public void Play(AudioSource source)
    {

        if (timestamp > Time.time) return;
        timestamp = Time.time + Cooldown;

        source.volume = Random.Range(VolumeMin, VolumeMax);
        source.pitch = Random.Range(PitchMin, PitchMax);
        source.loop = Loop;

        if (Clips.Count == 1)
            source.clip = Clips[0];
        else
        {
            currClip = Random.Range(0, Clips.Count);
            while (currClip == lastClip)
            {
                currClip = Random.Range(0, Clips.Count);
            }
            source.clip = Clips[currClip];
            lastClip = currClip;
        }

        source.Play();

    }

    public void Play(Vector3 pos)
    {
        if (AudioSourcePool.Instance == null) return;
        if (Clips.Count <= 0) return;
        Play(AudioSourcePool.Instance.GetSource(), pos);

    }

    public void Play(AudioSource source, Vector3 pos)
    {
        if (timestamp > Time.time) return;
        timestamp = Time.time + Cooldown;

        source.volume = Random.Range(VolumeMin, VolumeMax);
        source.pitch = Random.Range(PitchMin, PitchMax);
        source.loop = Loop;
        source.transform.position = pos;

        if (Clips.Count == 1)
            source.clip = Clips[0];
        else
        {
            currClip = Random.Range(0, Clips.Count);
            while (currClip == lastClip)
            {
                currClip = Random.Range(0, Clips.Count);
            }
            source.clip = Clips[currClip];
            lastClip = currClip;
        }

        AudioSource.PlayClipAtPoint(source.clip, pos, 1.0f);
    }
}