using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoad : MonoBehaviour
{
    public GameObject MenuPanel;
    public GameObject tutorPanel;
    public GameObject settingPanel;
    public GameObject creditPanel;
    public GameObject LosePanel;

    //Game Level
    public GameObject pausePanel;
    public GameObject panelMenang;
    public GameObject panelKalah;

    public GameObject panelBusway;

    public GameObject movBtn;
    public static SceneLoad Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        if (MenuPanel == null)
        {
            return;
        }
        if (tutorPanel == null)
        {
            return;
        }
        if (creditPanel == null)
        {
            return;
        }
        
    }
    public void NewGame()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1f;
    }

    public void TutorPanel()
    {
        MenuPanel.SetActive(false);
        settingPanel.SetActive(false);
        creditPanel.SetActive(false);
        tutorPanel.SetActive(true);
    }

    public void SettingPanel()
    {
        MenuPanel.SetActive(false);
        settingPanel.SetActive(true);
        creditPanel.SetActive(false);
        tutorPanel.SetActive(false);
    }
   
    public void CreditPanel()
    {
        MenuPanel.SetActive(false);
        settingPanel.SetActive(false);
        creditPanel.SetActive(true);
        tutorPanel.SetActive(false);
    }

    public void Back()
    {
        MenuPanel.SetActive(true);
        settingPanel.SetActive(false);
        creditPanel.SetActive(false);
        tutorPanel.SetActive(false);
    }

    public void BackMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Pause()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
    }
    public void Resume()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }
        if(panelBusway == true)
        {
            movBtn.SetActive(false);
        }
        else
        {
            movBtn.SetActive(true);
        }
    }
}
