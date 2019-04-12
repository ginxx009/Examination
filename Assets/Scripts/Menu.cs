﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Menu : MonoBehaviour
{
    public GameObject[] menus;
    public GameObject credits;

    public void Play()
    {
        SceneManager.LoadScene("Word Scramble", LoadSceneMode.Single);
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