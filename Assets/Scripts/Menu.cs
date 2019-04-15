using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Menu : MonoBehaviour
{
    public GameObject[] menus;
    public GameObject credits;
    public GameObject mode;


    //private void Start()
    //{
    //    PlayerPrefs.DeleteKey("Achievement1");
    //}

    public void Play()
    {
        foreach (GameObject menu in menus)
        {
            menu.SetActive(false);
        }
        mode.SetActive(true);
    }

    public void Endless()
    {
        
        SceneManager.LoadScene("Word Scramble Endless", LoadSceneMode.Single);
        
        if (PlayerPrefs.HasKey("Achievement1"))
            Debug.Log("already has key");
        else
            StartCoroutine(Achievement.instance.ShowAchievement());

        PlayerPrefs.SetString("Achievement1", "one");
        PlayerPrefs.Save();

    }

    public void Timer()
    {
        
        SceneManager.LoadScene("Word Scramble Time", LoadSceneMode.Single);
        
        if (PlayerPrefs.HasKey("Achievement1"))
            Debug.Log("already has key");
        else
            StartCoroutine(Achievement.instance.ShowAchievement());

        PlayerPrefs.SetString("Achievement1", "one");
        PlayerPrefs.Save();

    }

    public void Credits()
    {
        foreach(GameObject menu in menus)
        {
            menu.SetActive(false);
        }
        credits.SetActive(true);
    }

    public void Back()
    {
        foreach (GameObject menu in menus)
        {
            menu.SetActive(true);
        }
        credits.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
