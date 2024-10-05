using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.UI;
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

    /// <summary>
    /// Opens the stats menu
    /// </summary>
    public void Open()
    {
        isMenuOpen = true;
        Time.timeScale = 0f;
        canvas.enabled = true;
        UpdateLevelPoints();
    }

    /// <summary>
    ///  Closes the stats menu
    /// </summary>
    public void Close()
    {
        isMenuOpen = false;
        Time.timeScale = 1f;
        canvas.enabled = false;
    }

    /// <summary>
    /// Gets Upgrade Points (codenamed levelPoints) from PlayerStats so it can update the UI of the menu
    /// </summary>
    public void UpdateLevelPoints()
    {
        levelPoints = playerStats.levelPoints;
        upgradePointsText.text = "Upgrade Points: " + levelPoints;
        var upgradeButtons = gameObject.GetComponentsInChildren<UpgradeButtons>();
        foreach (var i in upgradeButtons)
        {
            // maxLevel has to be -1 as the array has "points to get to next level"
            // As level 6 doesn't exist, maxLevel[5] doesn't also
            if (i.upgradeCost[Mathf.Clamp(i.currentLevel,1,i.maxLevel-1)] > levelPoints || i.currentLevel >= 5)
            {
                i.upgradeButton.button.interactable = false;
                i.upgradeButton.value.color = new Color(0, 0.529f, 0.318f);
                i.upgradeButton.symbol.enabled = false;
            }
            else
            {
                i.upgradeButton.button.interactable = true;
                i.upgradeButton.value.color = Color.white;
                i.upgradeButton.symbol.enabled = true;
            }
        }
    }
}
