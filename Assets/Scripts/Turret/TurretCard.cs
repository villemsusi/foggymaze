using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class TurretCard : MonoBehaviour
{
    public TurretData TurretData;

    public TextMeshProUGUI ShortCutText;

    private void Awake()
    {
        if (TurretData != null)
        {
            ShortCutText.text = TurretData.Shortcut;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(TurretData.Shortcut))
        {
            Events.SelectTurret(TurretData);
        }
    }

    public void Pressed()
    {
        Events.SelectTurret(TurretData);
    }
}
