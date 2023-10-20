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

    private void Awake()
    {
        Selections = transform.GetChild(1).gameObject;
        Selection1 = Selections.transform.GetChild(0).GetComponent<Button>();
        Selection2 = Selections.transform.GetChild(1).GetComponent<Button>();
        Selection3 = Selections.transform.GetChild(2).GetComponent<Button>();

        Selection1.onClick.AddListener(SetHealth);
        Selection2.onClick.AddListener(SetMovespeed);
        Selection3.onClick.AddListener(SetSomething);
    }

    private void SetHealth()
    {
        Events.SetHealthPerm(Events.GetHealthPerm() * 2);
        Events.NextStage();
    }
    private void SetMovespeed()
    {
        Events.SetMovespeedPerm(Events.GetMovespeedPerm() * 2);
        Events.NextStage();
    }
    private void SetSomething()
    {
        // To-Do
        // Set third augment to do something
        Events.NextStage();
    }
}
