using ToolBox.Serialization;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class SaveDataUtilities : MonoBehaviour
{
    
}

/// <summary>
/// Use this class to store variables for the save system, this prevents code to break due to typo or changed name
/// </summary>
public class SaveDataKeywords
{
    // Global
    public const string score = "scoreCurrentScore";
    public const string highScore = "scoreHighScore";
    public const string goldCoins = "goldCoins";

    // Abilities
    public const string dashBought = "itemDashBought";
    public const string dashEquipped = "itemDashEquipped";
    public const string shieldBought = "itemShieldBought";
    public const string shieldEquipped = "itemShieldEquipped";
    public const string healthBought = "itemHealthBought";
    public const string healthLevel = "itemHealthLevel";
    
    // Wepons
    public const string burstBought = "itemWeaponBurstBought";
    public const string tripleBought = "itemWeaponTripleBought";
    public const string weaponEquiped = "itemWeaponEquiped";

    // Stat
    public const string statDamage = "statDamageMultiplierLevel";
    public const string statFire = "statFireRateLevel";
    public const string statBullet = "statBulletSpeedMultiplierLevel";
    public const string statCrit = "statCriticalHitProbabilityLevel";
    public const string statSpeed = "statSpeedMultiplierLevel";
    
    // Others
    public const string playerPosition = "playerCurrentPosition";
}

#if UNITY_EDITOR
[CustomEditor(typeof(SaveDataUtilities))]
public class SaveDataUtilitiesEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var saveDataUtilities = (SaveDataUtilities)target;
        if (saveDataUtilities == null) return;
        
        Undo.RecordObject(saveDataUtilities, "SaveDataUtilities");

        if (GUILayout.Button("Delete All Saved Data"))
        {
            DataSerializer.DeleteAll();
        }
    }
}
#endif