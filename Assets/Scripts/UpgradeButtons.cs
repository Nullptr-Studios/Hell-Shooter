using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButtons : MonoBehaviour
{
    [Header("Stats")]
    public StatID statId;
    public int maxLevel = 5;
    [Tooltip("Please do not use ID 0")] // TODO: Find a way to fix this, maybe with a try catch
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
    
    /// <summary>
    /// Logic for leveling up a player statistic
    /// </summary>
    public void LevelUp()
    {
        // Clamp is not needed but is there for a precaution
        currentLevel= Mathf.Clamp(currentLevel, 1, maxLevel);
        player.BroadcastMessage("GiveLevelPoint", -upgradeCost[currentLevel]);
        player.BroadcastMessage("StatLevelUp", statId);
        currentLevel++;

        // All fom here is UI related
        levelSlider.value = (float)currentLevel/maxLevel;
        levelText.text = currentLevel.ToString();
        upgradeButton.GetComponentInChildren<TextMeshProUGUI>().text = upgradeCost[currentLevel].ToString();
        downgradeButton.GetComponentInChildren<TextMeshProUGUI>().text = upgradeCost[currentLevel-1].ToString();
        transform.parent.gameObject.BroadcastMessage("UpdateLevelPoints");
        if (currentLevel == maxLevel) // This disables the buttons on reach level limit
            upgradeButton.interactable = false;
        if (downgradeButton.interactable == false) // This enabled the downgrade button again if disabled
            downgradeButton.interactable = true;
        // TODO: I dont think these are necessary since there is the UpdateLevelPoints
    }
    
    /// <summary>
    /// Logic for leveling down a player statistic
    /// </summary>
    public void LevelDown()
    {
        // This function is really the same as LevelUp(), but I can't put parameters for buttons, so I have to do it like this
        // Unity is a great engine ofc -x
        currentLevel = Mathf.Clamp(currentLevel, 1, maxLevel);
        player.BroadcastMessage("StatLevelDown", statId);
        player.BroadcastMessage("GiveLevelPoint", upgradeCost[currentLevel-1]);
        currentLevel--;

        // All fom here is UI related
        levelSlider.value = (float)currentLevel/maxLevel;
        levelText.text = currentLevel.ToString();
        downgradeButton.GetComponentInChildren<TextMeshProUGUI>().text = upgradeCost[currentLevel-1].ToString();
        upgradeButton.GetComponentInChildren<TextMeshProUGUI>().text = upgradeCost[currentLevel].ToString();
        transform.parent.gameObject.BroadcastMessage("UpdateLevelPoints");
        if (currentLevel == 1) // This disables the buttons on reach level limit
            downgradeButton.interactable = false;
        if (upgradeButton.interactable == false) // This enabled the upgrade button again if disabled
            upgradeButton.interactable = true;
    }
}
