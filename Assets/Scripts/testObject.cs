﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class testObject : MonoBehaviour
{
    public char character;
    public Text text;
    public Image image;
    public RectTransform rectTransform;
    public int index;

    [Header("Apperance")]
    public Color normalColor;
    public Color selectedColor;

    bool isSelected = false;

    public testObject Init(char c)
    {
        character = c;
        text.text = c.ToString();
        gameObject.SetActive(true);
        return this;
    }

    public void Select()
    {
        isSelected = !isSelected;

        image.color = isSelected ? selectedColor : normalColor;
        if (isSelected)
        {
            testScript.main.Select(this);
        }
        else
        {
            testScript.main.UnSelect();
        }
    }
}
