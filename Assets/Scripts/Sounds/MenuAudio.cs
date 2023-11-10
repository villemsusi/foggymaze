using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAudio : MonoBehaviour
{

    public AudioClipGroup ClickAudio;


    public void PlayClickAudio()
    {
        ClickAudio.Play();
    }

}
