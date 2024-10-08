using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public TextMeshProUGUI TurretCountText;
    public TextMeshProUGUI UpgradeCountText;
    public TextMeshProUGUI ReloadCountText;

    public TextMeshProUGUI Timer;
    public TextMeshProUGUI Alert;
    public TextMeshProUGUI Level;

    public GameObject PauseScreen;
    
    
    private int textChangeScale = 5;



    private void Awake()
    {
        Events.OnSetTurretCount += SetTurretText;
        Events.OnSetUpgradeCount += SetUpgradeText;
        Events.OnSetAmmoCount += SetReloadText;

        Events.OnSetTimer += SetTimer;
        Alert.gameObject.SetActive(false);
    }

    private void Start()
    {
        SetLevel(Events.GetLevelProgress());
    }

    private void OnDestroy()
    {
        Events.OnSetTurretCount -= SetTurretText;
        Events.OnSetUpgradeCount -= SetUpgradeText;
        Events.OnSetAmmoCount -= SetReloadText;

        Events.OnSetTimer -= SetTimer;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }


    private void SetTurretText(int amount)
    {
        TurretCountText.text = amount.ToString();
        StartCoroutine(TextChange(TurretCountText));
    }
    private void SetUpgradeText(int amount)
    {
        UpgradeCountText.text = amount.ToString();
        StartCoroutine(TextChange(UpgradeCountText));
    }
    private void SetReloadText(int amount)
    {
        ReloadCountText.text = amount.ToString();
        StartCoroutine(TextChange(ReloadCountText));
    }

    private void SetTimer(float amount)
    {
        if (amount == 0)
        {
            if (!Alert.gameObject.activeSelf)
                Alert.gameObject.SetActive(true);
            else
                return;
            Timer.text = "";
            Alert.color = Color.red;
            Alert.text = "ESCAPE!";
            InvokeRepeating(nameof(AlertFlash), 0.5f, 0.5f);
            return;
        }
        else if (Events.GetStageStartTimer() - amount < 5f)
        {
            if (!Alert.gameObject.activeSelf)
                Alert.gameObject.SetActive(true);
            Alert.color = Color.red;
            Alert.text = "SURVIVE!";
            InvokeRepeating(nameof(AlertFlash), 1f, 1f);            
            
        }
        else
        {
            if (Alert.gameObject.activeSelf)
            {
                
                Alert.gameObject.SetActive(false);
                CancelInvoke();
            }
            
        }
            
        Timer.text = Mathf.RoundToInt(amount).ToString();
    }
    private void AlertFlash()
    {
        if (Alert.color == Color.red)
            Alert.color = new Color(0.3867925f, 0.03831435f, 0.03831435f);
        else
            Alert.color = Color.red;
    }


    private void SetLevel(int amount)
    {
        Level.text = ToRoman(amount);
    }


    private IEnumerator TextChange(TextMeshProUGUI text)
    {
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime;
            if (t > 1) t = 1;

            float scale = -4 * (textChangeScale - 1) * Mathf.Pow(t, 2) + (4 * (textChangeScale - 1) * t) + 1;
            text.transform.localScale = new Vector3(scale, scale, 1);

            yield return new WaitForEndOfFrame();
            
        }
    }

    private void TogglePause()
    {
        if (PauseScreen.activeSelf)
        {
            PauseScreen.SetActive(false);
            Time.timeScale = 1;
            return;
        }
        if (!(Time.timeScale == 0))
        {
            Events.SetTrauma(0f);
            PauseScreen.SetActive(true);
            Time.timeScale = 0;
        }
            
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
