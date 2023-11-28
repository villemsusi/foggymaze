using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public bool MX;
    public bool SFX;

    public List<AudioClip> Clips;


    private float timestamp;
    private int lastClip;
    private int currClip;

    private void OnEnable()
    {
        timestamp = 0;
        lastClip = 0;
    }

    public AudioSource Play()
    {
        if (AudioSourcePool.Instance == null) return null;
        if (Clips.Count <= 0) return null;
        AudioSource source = AudioSourcePool.Instance.GetSource();
        Play(source);
        return source;
        

    }

    public void Play(AudioSource source)
    {

        if (timestamp > Time.time) return;
        timestamp = Time.time + Cooldown;

        if (MX)
            source.volume = Random.Range(VolumeMin, VolumeMax) * Events.GetMX();
        else if (SFX)
            source.volume = Random.Range(VolumeMin, VolumeMax) * Events.GetSFX();
        else
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

        if (SFX)
            source.volume = Random.Range(VolumeMin, VolumeMax) * Events.GetSFX();
        else if (MX)
            source.volume = Random.Range(VolumeMin, VolumeMax) * Events.GetMX();
        else
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

        AudioSource.PlayClipAtPoint(source.clip, pos, Events.GetSFX());
    }
}