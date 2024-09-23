using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButtons : MonoBehaviour
{
    [Header("Stats")]
    public StatID statId;
    public int maxLevel = 1;
    public int[] upgradeCost;
    [NonSerialized] public int currentLevel = 1;
    [Header("Buttons")]
    public Button upgradeButton;
    public Button downgradeButton;
    public Slider levelSlider;
    public TextMeshProUGUI levelText;
    
    private GameObject player;
    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        downgradeButton.interactable = false;
        levelSlider.value = (float)currentLevel/maxLevel;
        upgradeButton.GetComponentInChildren<TextMeshProUGUI>().text = upgradeCost[currentLevel].ToString();
        downgradeButton.GetComponentInChildren<TextMeshProUGUI>().text = upgradeCost[currentLevel-1].ToString();
    }
    
    public void LevelUp()
    {
        Mathf.Clamp(currentLevel, 1, maxLevel);
        player.BroadcastMessage("StatLevelUp", statId);
        levelSlider.value = (float)currentLevel/maxLevel;
        levelText.text = currentLevel.ToString();
        player.BroadcastMessage("GiveLevelPoint", -upgradeCost[currentLevel]); // Function adds so here is substracting
        currentLevel++;
        upgradeButton.GetComponentInChildren<TextMeshProUGUI>().text = upgradeCost[currentLevel].ToString();
        downgradeButton.GetComponentInChildren<TextMeshProUGUI>().text = upgradeCost[currentLevel-1].ToString();
        transform.parent.gameObject.BroadcastMessage("UpdateLevelPoints");
        // This disables the buttons on reach level limit
        if (currentLevel == maxLevel)
            upgradeButton.interactable = false;
        // This enabled the downgrade button again if disabled
        if (downgradeButton.interactable == false)
            downgradeButton.interactable = true;
    }
    public void LevelDown()
    {
        Mathf.Clamp(currentLevel, 1, maxLevel);
        player.BroadcastMessage("StatLevelDown", statId);
        levelSlider.value = (float)currentLevel/maxLevel;
        levelText.text = currentLevel.ToString();
        player.BroadcastMessage("GiveLevelPoint", upgradeCost[currentLevel-1]);
        currentLevel--;
        downgradeButton.GetComponentInChildren<TextMeshProUGUI>().text = upgradeCost[currentLevel-1].ToString();
        upgradeButton.GetComponentInChildren<TextMeshProUGUI>().text = upgradeCost[currentLevel].ToString();
        transform.parent.gameObject.BroadcastMessage("UpdateLevelPoints");
        // This disables the buttons on reach level limit
        if (currentLevel == 1)
            downgradeButton.interactable = false;
        // This enabled the upgrade button again if disabled
        if (upgradeButton.interactable == false)
            upgradeButton.interactable = true;
    }
}
