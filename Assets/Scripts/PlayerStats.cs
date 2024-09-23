using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStats : MonoBehaviour
{
    [Header("Stat System")]
    [Range(0.9f, 1.5f)] public float statEffectMultiplier = 1f;
    [Range(0.1f, 0.9f)] public float statXpRatio = 0.25f;
    [Range(1f, 4f)] public float statXpMultiplier = 1.63f;
    private int[] StatLevel = new int[Enum.GetValues(typeof(StatID)).Length];
    private float[] StatMultiplier = new float[Enum.GetValues(typeof(StatID)).Length];

    [Header("XP System")]
    public int requiredXP;
    // Made protected so XP and Level can only be changed by GiveXP() function
    [NonSerialized] protected int xp;
    [NonSerialized] protected int levelPoints = 0;
    
    PlayerInput input;

    private void Start()
    {
        // Initializes all stats to 1
        for (int i = 0; i < StatLevel.Length; i++)
        {
            StatLevel[i] = 1;
            StatMultiplier[i] = 1f;
        }
        
        input = GetComponent<PlayerInput>();
    }

    /**
     *  Increases the level of the inputted stat by 1
     *  Right now stats don't have a hardcoded limit, they can be leveled up until infinite
     *  
     *  <param name="statID">Ability type</param>
     */
    public void StatLevelUp(StatID statID)
    {
        var id = (int)statID;
        StatLevel[id]++;
        StatMultiplier[id] = (statEffectMultiplier - statXpRatio) + statXpRatio * Mathf.Pow(statXpMultiplier, StatLevel[id]-1);
        DebugPrint(statID);
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
        StatMultiplier[id] = (statEffectMultiplier - statXpRatio) + statXpRatio * Mathf.Pow(statXpMultiplier, StatLevel[id] - 1);
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
    public float GetStatLevel(StatID statID) => StatLevel[(int)statID];

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
     */
    public void GiveXP(int _xp)
    {
        xp += _xp;
        while (xp >= requiredXP)
        {
            xp -= requiredXP;
            levelPoints++;
        }
    }

    /**
     *  Calls level menu to be opened and switches ActionMap to the one for LevelMenu
     */  
    private void OnOpenLevelMenu()
    {
        input.SwitchCurrentActionMap("UI");
        LevelMenu levelMenuScript = GameObject.Find("LevelMenu").GetComponent<LevelMenu>();
        levelMenuScript.Open();
    }

    /**
     *  Calls level menu to be closed and switches ActionMap back to default (gameplay)
     */  
    private void OnCloseLevelMenu()
    {
        input.SwitchCurrentActionMap("Gameplay");
        LevelMenu levelMenuScript = GameObject.Find("LevelMenu").GetComponent<LevelMenu>();
        levelMenuScript.Close();
    }
}

/**
 *  Enum that contains all stats the player has
 *  Updating this will automatically change the Arrays of the player, so it's easy to scale and add
 *  new stats to the player
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
 