using System;
using ToolBox.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerStats : MonoBehaviour
{
    // Privates
    PlayerInput input;

    private int _score;
    private float _scoreTimer;
    
    public int Score { get => _score; }

    [Header("Stat System")]
    [Range(0.9f, 1.5f)] public float statEffectMultiplier = 1f;
    [Range(1f, 8f)] public float statXpCurve = 5f;
    private int[] StatLevel = new int[Enum.GetValues(typeof(StatID)).Length];
    private float[] StatMultiplier = new float[Enum.GetValues(typeof(StatID)).Length];

    [Header("XP System")]
    public int requiredXP;
    // Made protected so XP and Level can only be changed by GiveXP() function
    [NonSerialized] protected internal int xp;
    [NonSerialized] protected internal int levelPoints = 0;
    public int goldCoins;

    private GameObject GUI;

#if UNITY_EDITOR
    [Header("Debug")]
    public bool logLevelUp;
    public bool logSave = false;
#endif

    private void Start()
    {
        // Initializes all stats to 1
        for (int i = 0; i < StatLevel.Length; i++)
        {
            StatLevel[i] = 1;
            StatMultiplier[i] = 1f;
        }

        _score = DataSerializer.Load<int>(SaveDataKeywords.score);

        input = GetComponent<PlayerInput>();
        input.onActionTriggered += OnOpenLevelMenu;
        input.onActionTriggered += OnCloseLevelMenu;

        GUI = GameObject.Find("GUI");

        xp = 0;
        GUI.SendMessage("SetXpBar", xp / requiredXP);
        GUI.SendMessage("SetLevelPoints", levelPoints);

        // Get data from save file
        goldCoins = DataSerializer.Load<int>(SaveDataKeywords.goldCoins);

#if UNITY_EDITOR
        if (logSave) Debug.Log("Loaded gold coins: " + goldCoins);
#endif

    }

    private void Update()
    {
        _scoreTimer += Time.deltaTime;

        if (_scoreTimer >= 1)
        {
            GiveScore(1);
            _scoreTimer = 0;
        }
    }

    /**
     *  Increases the level of the inputted stat by 1
     *  Right now stats don't have a hardcoded limit, they can be leveled up until infinite
     *
     *  If you check the formula please update StatLevelUp also
     *  
     *  <param name="statID">Ability type</param>
     */
    public void StatLevelUp(StatID statID)
    {
        var id = (int)statID;
        StatLevel[id]++;
        StatMultiplier[id] = statEffectMultiplier * (1 + Mathf.Log(StatLevel[id], statXpCurve));
        //if (logLevelUp) DebugPrint(statID);
    }

    /**
     *  Decrease the level of the inputted stat by 1
     *  
     *  <param name="statID">Ability type</param>
     */
    public void StatLevelDown(StatID statID)
    {
        var id = (int)statID;
        StatLevel[id]--;
        StatMultiplier[id] = statEffectMultiplier * (1 + Mathf.Log(StatLevel[id], statXpCurve));

#if UNITY_EDITOR
        if (logLevelUp) DebugPrint(statID);
#endif

    }

    /**
     *  Returns current multiplier for inputted stat
     *  
     *  <param name="statID">Ability type</param>
     *  <returns>Multiplier for the stat</returns>
     */
    public float GetStat(StatID statID) => StatMultiplier[(int)statID];

    /**
     *  Returns current level for inputted stat
     *  
     *  <param name="statID">Ability type</param>
     *  <returns>Level of the stat</returns>
     */
    public int GetStatLevel(StatID statID) => StatLevel[(int)statID];

    /**
     *  Prints the level and the applied multiplayer of the inputted stat
     *  WON'T WORK ON RELEASE
     *  
     *  <param name="statID">Ability type</param>
     */
    public void DebugPrint(StatID statID)
    {
        Debug.Log(statID + " (" + (int)statID + "):: Level: " + StatLevel[(int)statID] + " Multiplier: " + StatMultiplier[(int)statID]);
    }

    /**
     *  Gives the player the XP sent in the parameter. Also checks for level up.
     *
     *  <param name="_xp">XP given to the player</param>>
     */
    public void GiveXP(int _xp)
    {
        // Prevents infinite loop if required xp is 0 or less (thanks unity for crashing btw) -x
        if (requiredXP <= 0)
        {
            Debug.LogWarning("XP not given since required XP is zero or less. Check requiredXP to be able to use GiveXP().");
            return;
        }

        xp += _xp;
        while (xp >= requiredXP)
        {
            xp -= requiredXP;
            levelPoints++;
            GUI.SendMessage("SetLevelPoints", levelPoints);
        }

        // Broadcast to GUI slider
        GUI.SendMessage("SetXpBar", (float)xp / requiredXP);
    }

    /**
     *  Grants the player a level point
     *  Used when the player lowers one of their stats
     *
     *  <param name="number">Number of Level Points given to player</param>
     */
    public void GiveLevelPoint(int number) 
    {
        levelPoints += number;
        GUI.SendMessage("SetLevelPoints", levelPoints);
    }

    /// <summary>
    /// Gives player Gold Coins
    /// </summary>
    /// <param name="value">Number of gold coins to give</param>
    public void GiveGold(int value) => goldCoins += value;

    /**
     *  Calls level menu to be opened and switches ActionMap to the one for LevelMenu
     */  
    private void OnOpenLevelMenu(InputAction.CallbackContext context)
    {
        if (context.action.name != "OpenLevelMenu" || !context.performed)
            return;
        
        input.SwitchCurrentActionMap("UI");
        LevelMenu levelMenuScript = GameObject.Find("LevelMenu").GetComponent<LevelMenu>();
        levelMenuScript.Open();
    }

    /**
     *  Calls level menu to be closed and switches ActionMap back to default (gameplay)
     */  
    private void OnCloseLevelMenu(InputAction.CallbackContext context)
    {
        if (context.action.name != "CloseLevelMenu" || !context.performed)
            return;
        
        input.SwitchCurrentActionMap("Gameplay");
        LevelMenu levelMenuScript = GameObject.Find("LevelMenu").GetComponent<LevelMenu>();
        levelMenuScript.Close();
    }

    /// <summary>
    ///  Gives the player score for the leaderboard
    /// </summary>
    /// <param name="value">Number of points to be added</param>
    public void GiveScore(int value)
    {
        _score += value;
        GUI.SendMessage("UpdateScore", _score);
    }

    /// <summary>
    /// Saves values onto file
    /// </summary>
    private void SaveData()
    {
        DataSerializer.Save(SaveDataKeywords.goldCoins, goldCoins);
        DataSerializer.Save(SaveDataKeywords.score, _score);
        
        DataSerializer.Save(SaveDataKeywords.statBullet, StatLevel[(int)StatID.bulletSpeedMultiplier]);
        DataSerializer.Save(SaveDataKeywords.statCrit, StatLevel[(int)StatID.criticalHitPercentage]);
        DataSerializer.Save(SaveDataKeywords.statDamage, StatLevel[(int)StatID.damageMultiplier]);
        DataSerializer.Save(SaveDataKeywords.statFire, StatLevel[(int)StatID.fireRateMultiplier]);
        DataSerializer.Save(SaveDataKeywords.statSpeed, StatLevel[(int)StatID.speedMultiplier]);
        
        DataSerializer.Save(SaveDataKeywords.playerPosition, transform.position);
        
#if UNITY_EDITOR
        if (logSave) Debug.Log("Data saved: " + DataSerializer.Load<int>(SaveDataKeywords.goldCoins));
#endif
        
    }
}

/**
 *  Enum that contains all stats the player has
 *  Updating this will automatically change the Arrays of the player, so it's easy to scale and add
 *  new stats to the player
 *
 *  Updating this will make you need to update the store page, since that UI is unrelated from this file
 *  Check classes LevelMenu and UpgradeButtons for that
 */
public enum StatID
{
    damageMultiplier = 0,
    fireRateMultiplier = 1,
    bulletSpeedMultiplier = 2,
    criticalHitPercentage = 3,
    speedMultiplier = 4,
}

/*  TODO: Make this a custom unity component using child of class ScriptableObject
 *  Example:
 *  [CreateAssetMenu(fileName = "New MotionCurve", menuName = "Create MotionCurve")]
 *  public class MotionCurve : ScriptableObject {};
 */  
 