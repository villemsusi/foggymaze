using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAudio : MonoBehaviour
{
    public void PlayClickAudio()
    {
        DataManager.Instance.ClickAudio.Play();
    }

    public void PlayAugmentSelectAudio()
    {
        DataManager.Instance.ClickAudio.Play();
    }

}
