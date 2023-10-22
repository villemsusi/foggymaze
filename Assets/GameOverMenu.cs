using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverMenu : MonoBehaviour
{

    public TextMeshProUGUI ScoreText;

    // Start is called before the first frame update
    void Start()
    {
        ScoreText.text = Events.GetLevelProgress().ToString();
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
