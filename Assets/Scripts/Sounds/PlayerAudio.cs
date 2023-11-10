using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    public AudioClipGroup WalkAudio;

    public void PlayWalkSound()
    {
        Debug.Log("PLAYCALL");
        WalkAudio.Play();
    }
}
