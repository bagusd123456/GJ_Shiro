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

    //Game Level
    public GameObject pausePanel;
    public GameObject panelMenang;
    public GameObject panelKalah;

    public GameObject panelBusway;

    //Power
    public GameObject powerNotif;
    public GameObject speedUp;
    public GameObject armorUp;
    public GameObject attackUp;


    public GameObject movBtn;
    public static SceneLoad Instance { get; private set; }

    private int currentSceneIndex;
    private int sceneToContinue;

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
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        PlayerPrefs.SetInt("SavedScene", currentSceneIndex);
        SceneManager.LoadScene(0);
    }

    public void ContinueGame()
    {
        sceneToContinue = PlayerPrefs.GetInt("SavedScene");

        if (sceneToContinue != 0)
            SceneManager.LoadScene(sceneToContinue);
        else
            return;
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

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }
        if(panelBusway == true)
        {
            //movBtn.SetActive(false);
        }
        else
        {
            //movBtn.SetActive(true);
        }
    }

    public void GetPower(int type)
    {
        StartCoroutine(PowerTime(type));
    }
    IEnumerator PowerTime(int type)
    {
        powerNotif.SetActive(true);
        switch (type)
        {
            case 0:
                speedUp.SetActive(true);
                break;

            case 1:
                armorUp.SetActive(true);
                break;
            case 2:
                attackUp.SetActive(true);
                break;

            default:
                break;
        }
        yield return new WaitForSeconds(1f);
        powerNotif.SetActive(false);
        speedUp.SetActive(false);
        armorUp.SetActive(false);
        attackUp.SetActive(false);
    }
}
