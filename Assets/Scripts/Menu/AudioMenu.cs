using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioMenu : MonoBehaviour
{

    [SerializeField] private Slider sfx;
    [SerializeField] private Slider mx;

    public void SetSFXVolume()
    {
        Debug.Log(sfx.value);
        Events.SetSFX(sfx.value);
        DataManager.Instance.ClickAudio.Play();
    }

    public void SetMXVolume()
    {
        Debug.Log(mx.value);
        Events.SetMX(mx.value);
        DataManager.Instance.Music.volume = DataManager.Instance.BckgrMusic.VolumeMax * mx.value;
    }
}
