using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButtons : MonoBehaviour
{
    [Header("Stats")]
    public StatID statId;
    public int maxLevel = 1;
    [SerializeField] protected int currentLevel = 1;
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
    }
    public void LevelUp()
    {
        Mathf.Clamp(currentLevel, 1, maxLevel);
        player.BroadcastMessage("StatLevelUp", statId);
        currentLevel++;
        levelSlider.value = (float)currentLevel/maxLevel;
        levelText.text = currentLevel.ToString();
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
        currentLevel--;
        levelSlider.value = (float)currentLevel/maxLevel;
        levelText.text = currentLevel.ToString();
        // This disables the buttons on reach level limit
        if (currentLevel == 1)
            downgradeButton.interactable = false;
        // This enabled the upgrade button again if disabled
        if (upgradeButton.interactable == false)
            upgradeButton.interactable = true;
    }
}
