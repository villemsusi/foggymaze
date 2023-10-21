using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUD : MonoBehaviour
{
    public TextMeshProUGUI TurretCountText;
    public TextMeshProUGUI UpgradeCountText;
    public TextMeshProUGUI ReloadCountText;

    private void Awake()
    {
        Events.OnSetTurretCount += SetTurretText;
        Events.OnSetUpgradeCount += SetUpgradeText;
        Events.OnSetAmmoCount += SetReloadText;
    }

    private void OnDestroy()
    {
        Events.OnSetTurretCount -= SetTurretText;
        Events.OnSetUpgradeCount -= SetUpgradeText;
        Events.OnSetAmmoCount -= SetReloadText;
    }


    private void SetTurretText(int amount)
    {
        TurretCountText.text = amount.ToString();
    }
    private void SetUpgradeText(int amount)
    {
        UpgradeCountText.text = amount.ToString();
    }
    private void SetReloadText(int amount)
    {
        ReloadCountText.text = amount.ToString();
    }

}
