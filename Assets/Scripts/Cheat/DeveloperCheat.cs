using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeveloperCheat : MonoBehaviour
{
    public static int clickCount = 0;
    public bool godMode = false;
    bool toggleCheatMenu = false;
    public GameObject cheatPanel;
    public GameObject cheatButton;

    // Update is called once per frame
    void Update()
    {
        if (godMode == true)
            FindObjectOfType<PlayerCondition>().currentHP = 100;

        if (toggleCheatMenu)
            cheatPanel.SetActive(true);
        else
            cheatPanel.SetActive(false);
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadPreviousScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void ToggleGodMode()
    {
        if(!godMode)
            godMode = true;
        else
            godMode = false;
    }

    public void TriggerCheat()
    {
        if(clickCount <= 5)
            clickCount++;
        else
        {
            toggleCheatMenu = true;
            cheatButton.SetActive(true);
        }
    }

    public void ToggleCheatMenu()
    {
        if (!toggleCheatMenu)
            toggleCheatMenu = true;
        else
            toggleCheatMenu = false;
    }
}
