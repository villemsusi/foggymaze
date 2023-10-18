using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class TowerCard : MonoBehaviour
{
    public TowerData TowerData;

    public TextMeshProUGUI ShortCutText;
    public Image IconImage;

    private Button button;
    private void Awake()
    {
        button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(Pressed);
        }
        if (TowerData != null)
        {
            ShortCutText.text = TowerData.Shortcut;
            //IconImage.sprite = TowerData.Icon;
        }
    }
    public void Pressed()
    {
        Events.SelectTower(TowerData);
    }
}
