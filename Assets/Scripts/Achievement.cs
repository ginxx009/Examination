﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Achievement : MonoBehaviour
{
    public static Achievement instance = null;

    public Text lbl;
    public Button achievement;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        achievement.GetComponent<Animator>().enabled = false;
    }

    public IEnumerator ShowAchievement()
    {
        achievement.GetComponent<Animator>().enabled = true;

        yield return null;

    }
}