using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUD : MonoBehaviour
{
    public TextMeshProUGUI TurretCountText;
    public TextMeshProUGUI UpgradeCountText;
    public TextMeshProUGUI ReloadCountText;

    public TextMeshProUGUI Timer;
    public TextMeshProUGUI Level;

    private void Awake()
    {
        Events.OnSetTurretCount += SetTurretText;
        Events.OnSetUpgradeCount += SetUpgradeText;
        Events.OnSetAmmoCount += SetReloadText;

        Events.OnSetTimer += SetTimer;
        Events.OnSetLevelProgress += SetLevel;
    }

    private void OnDestroy()
    {
        Events.OnSetTurretCount -= SetTurretText;
        Events.OnSetUpgradeCount -= SetUpgradeText;
        Events.OnSetAmmoCount -= SetReloadText;

        Events.OnSetTimer -= SetTimer;
        Events.OnSetLevelProgress -= SetLevel;
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

    private void SetTimer(int amount)
    {
        if (amount == 0)
        {
            Timer.color = Color.red;
            Timer.text = "ESCAPE!";
            Invoke(nameof(TimerFlash), 1f);
            return;
        }
        Timer.text = amount.ToString();
    }
    private void TimerFlash()
    {
        if (Timer.color == Color.red)
            Timer.color = new Color(0.3867925f, 0.03831435f, 0.03831435f);
        else
            Timer.color = Color.red;


        Invoke(nameof(TimerFlash), 1f);
    }


    private void SetLevel(int amount)
    {
        Level.text = ToRoman(amount);
    }

    public static string ToRoman(int number)
    {
        if ((number < 0) || (number > 3999)) return "Invalid Level Nr";
        if (number < 1) return string.Empty;
        if (number >= 1000) return "M" + ToRoman(number - 1000);
        if (number >= 900) return "CM" + ToRoman(number - 900);
        if (number >= 500) return "D" + ToRoman(number - 500);
        if (number >= 400) return "CD" + ToRoman(number - 400);
        if (number >= 100) return "C" + ToRoman(number - 100);
        if (number >= 90) return "XC" + ToRoman(number - 90);
        if (number >= 50) return "L" + ToRoman(number - 50);
        if (number >= 40) return "XL" + ToRoman(number - 40);
        if (number >= 10) return "X" + ToRoman(number - 10);
        if (number >= 9) return "IX" + ToRoman(number - 9);
        if (number >= 5) return "V" + ToRoman(number - 5);
        if (number >= 4) return "IV" + ToRoman(number - 4);
        if (number >= 1) return "I" + ToRoman(number - 1);
        return "";
    }

}
