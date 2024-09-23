using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour
{
    private Canvas canvas;
    private PlayerStats playerStats;
    
    private int levelPoints;
    [NonSerialized] public bool isMenuOpen = false;
    
    public TextMeshProUGUI upgradePointsText;

    private void Awake()
    {
        playerStats = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();

        canvas = gameObject.GetComponent<Canvas>();
        canvas.enabled = false;
    }

    public void Open()
    {
        isMenuOpen = true;
        Time.timeScale = 0f;
        canvas.enabled = true;
        UpdateLevelPoints();
    }

    public void Close()
    {
        isMenuOpen = false;
        Time.timeScale = 1f;
        canvas.enabled = false;
    }

    public void UpdateLevelPoints()
    {
        levelPoints = playerStats.levelPoints;
        upgradePointsText.text = "Upgrade Points: " + levelPoints;
        var _upgradeButtons = gameObject.GetComponentsInChildren<UpgradeButtons>();
        foreach (var i in _upgradeButtons)
        {
            if (i.upgradeCost[i.currentLevel] > levelPoints)
                i.upgradeButton.interactable = false;
            else 
                i.upgradeButton.interactable = true;
        }
    }
}
