using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class ConsoleText : MonoBehaviour
{
    public TextMeshProUGUI DebugText;

    public string output = "";
    public string stack = "";

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        output = logString;
        stack = stackTrace;

        DebugText.text += output + "<br>";
    }
}
