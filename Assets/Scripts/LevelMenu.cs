using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour
{
    private Canvas canvas;
    [NonSerialized] public bool isMenuOpen = false;

    private void Awake()
    {
        canvas = gameObject.GetComponent<Canvas>();
        canvas.enabled = false;
    }

    public void Open()
    {
        isMenuOpen = true;
        Time.timeScale = 0f;
        canvas.enabled = true;
    }

    public void Close()
    {
        isMenuOpen = false;
        Time.timeScale = 1f;
        canvas.enabled = false;
    }
}
