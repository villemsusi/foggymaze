using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Augment : MonoBehaviour
{

    private GameObject Selections;
    private Button Selection1;
    private Button Selection2;
    private Button Selection3;

    private float healthMultiplier;
    private float speedMultiplier;

    private void Awake()
    {
        Selections = transform.Find("Selections").gameObject;
        Selection1 = Selections.transform.GetChild(0).GetComponent<Button>();
        Selection2 = Selections.transform.GetChild(1).GetComponent<Button>();
        Selection3 = Selections.transform.GetChild(2).GetComponent<Button>();

        Selection1.onClick.AddListener(SetHealth);
        Selection2.onClick.AddListener(SetMovespeed);
        Selection3.onClick.AddListener(SetSomething);

        healthMultiplier = 1.2f;
        speedMultiplier = 1.06f;
    }

    private void SetHealth()
    {
        Events.SetHealthPerm((int)Mathf.Round(Events.GetHealthPerm() * healthMultiplier));
        Events.NextStage();
    }
    private void SetMovespeed()
    {
        Events.SetMovespeedPerm(Events.GetMovespeedPerm() * speedMultiplier);
        Events.NextStage();
    }
    private void SetSomething()
    {
        // To-Do
        // Set third augment to do something
        Events.NextStage();
    }
}
