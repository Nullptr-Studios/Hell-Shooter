using System;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    /**
     *  Enum that contains all stats the player has
     */
    public enum StatID
    {
        damageMultiplier = 0,
        fireRateMultiplier = 1,
        bulletSpeedMultiplier = 2,
        criticalHitPercentage = 3,
        speedMultiplier = 4,
    }
    private int[] StatLevel = new int[Enum.GetValues(typeof(StatID)).Length];
    private float[] StatMultiplier = new float[Enum.GetValues(typeof(StatID)).Length];

    private void Start()
    {
        // Initializes all stats to 1
        foreach (var i in StatLevel)
        {
            StatLevel[i] = 1;
            StatMultiplier[i] = 1f;
        }

    }

    /**
     *  Increases the level of the inputted stat by 1
     *  Right now stats don't have a hardcoded limit, they can be leveled up until infinite
     *  Scale formula right now is 0.75+0.25*1.63^(x-1), file with the graph can be opened with nspire calculator
     *  1 = x1; 2 = x1.157; 3 = x1.412; 4 = 1.832; 5 = x2.514 
     */
    public void LevelUp(StatID statID)
    {
        var id = (int)statID;
        StatLevel[id]++;
        StatMultiplier[id] = 1 + 0.25f * Mathf.Pow(1.63f, StatLevel[id]-1)-.25f;
    }
    
    /**
     *  Returns current multiplier for inputted stat
     */
    public float GetStat(StatID statID) => StatMultiplier[(int)statID];

    /**
     *  Prints the level and the applied multiplayer of the inputted stat
     *  WON'T WORK ON RELEASE
     */
    void DebugPrint(StatID statID)
    {
        Debug.Log(statID+" ("+(int)statID+"):: Level: "+StatLevel[(int)statID]+" Multiplier: "+StatMultiplier[(int)statID]);
    }
}
