using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButtons : MonoBehaviour
{
    [Header("Stats")]
    public StatID statId;
    public int maxLevel = 5;
    /**
     *  Value needed to upgrade to level menu
     *  INDEX WILL ALWAYS BE 1 LOWER THAN maxLevel
     */
    [Tooltip("Please do not use ID 0")]
    public int[] upgradeCost;
    public Sprite[] levelBarSprites;
    [NonSerialized] public int currentLevel = 1;
    [Header("Buttons")]
    [SerializeField]
    public UpgButton upgradeButton;
    [SerializeField] private UpgButton downgradeButton;
    public Image levelBar;
    
    private GameObject player;
    
    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        
        downgradeButton.button.interactable = false;
        downgradeButton.symbol.enabled = false;
        downgradeButton.value.color = new Color(1, 0, 0.302f);
        levelBar.sprite = levelBarSprites[currentLevel];
        
        upgradeButton.value.text = upgradeCost[currentLevel].ToString();
        downgradeButton.value.text = upgradeCost[currentLevel-1].ToString();
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
        levelBar.sprite = levelBarSprites[currentLevel];
        upgradeButton.value.text = upgradeCost[Mathf.Clamp(currentLevel, 1, maxLevel-1)].ToString();
        downgradeButton.value.text = upgradeCost[currentLevel-1].ToString();
        transform.parent.gameObject.BroadcastMessage("UpdateLevelPoints");
        if (downgradeButton.button.interactable == false) // This enabled the downgrade button again if disabled
        {
            downgradeButton.button.interactable = true;
            downgradeButton.value.color = Color.white;
            downgradeButton.symbol.enabled = true;
        }
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
        levelBar.sprite = levelBarSprites[currentLevel];
        downgradeButton.value.text = upgradeCost[currentLevel-1].ToString();
        upgradeButton.value.text = upgradeCost[currentLevel].ToString();
        transform.parent.gameObject.BroadcastMessage("UpdateLevelPoints");
        if (currentLevel <= 1) // This disables the buttons on reach level limit
        {
            downgradeButton.button.interactable = false;
            downgradeButton.value.color = new Color(1, 0, 0.302f);
            downgradeButton.symbol.enabled = false;
        }
    }
}

[System.Serializable]
public class UpgButton
{
    public Button button;
    public TextMeshProUGUI value;
    public TextMeshProUGUI symbol;
}

