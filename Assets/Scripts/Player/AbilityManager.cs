using ToolBox.Serialization;
using Unity.Burst;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class AbilityManager : MonoBehaviour
{
    // Abilities
    public Ability dash;
    public Ability shield;
    public Ability reinforcer;
    
    // Weapons
    public Ability defaultWeapon;
    public Ability burstWeapon;
    private int _equipedWeapon;
    [FormerlySerializedAs("aimbotWeapon")] public Ability tripleWeapon;
    
    void Awake()
    {
        // Instantiate abilities
        // Copy this for the rest of the abilities
        dash.isBought = DataSerializer.Load<bool>(SaveDataKeywords.dashBought);
        dash.isEquipped = DataSerializer.Load<bool>(SaveDataKeywords.dashEquipped);
        if (dash.isBought && dash.isEquipped)
            Instantiate(dash.prefab, this.transform);

        shield.isBought = DataSerializer.Load<bool>(SaveDataKeywords.shieldBought);
        shield.isEquipped = DataSerializer.Load<bool>(SaveDataKeywords.shieldEquipped);
        if (shield.isBought && shield.isEquipped)
            Instantiate(shield.prefab, this.transform);

        reinforcer.isBought = DataSerializer.Load<bool>(SaveDataKeywords.healthBought);
        reinforcer.isEquipped = DataSerializer.Load<bool>(SaveDataKeywords.healthBought); // The same because it cannot be unequipped once bought
        if (reinforcer.isBought)
            Instantiate(reinforcer.prefab, this.transform);
        
        burstWeapon.isBought = DataSerializer.Load<bool>(SaveDataKeywords.burstBought);
        tripleWeapon.isBought = DataSerializer.Load<bool>(SaveDataKeywords.tripleBought);
        switch (_equipedWeapon)
        {
            case 1:
                tripleWeapon.isEquipped = true;
                break;
            
            case 2:
                burstWeapon.isEquipped = true;
                break;
            
            default:
                defaultWeapon.isEquipped = true;
                break;
        }

        if (burstWeapon.isBought && burstWeapon.isEquipped)
            Instantiate(burstWeapon.prefab, this.transform);
        else if (tripleWeapon.isBought && tripleWeapon.isEquipped)
            Instantiate(tripleWeapon.prefab, this.transform);
        else
            Instantiate(defaultWeapon.prefab, this.transform);
    }
    
}

#if UNITY_EDITOR
[CustomEditor(typeof(AbilityManager))]
class AbilityManagerEditor : Editor
{
    /// <summary>
    /// This is all for the custom component buttons that appear on the inspector.
    /// This code is not included on build as there is no need to.
    /// </summary>
    public override void OnInspectorGUI()
    {
        var abilityManager = (AbilityManager)target;
        if (abilityManager == null) return;
        
        Undo.RecordObject(abilityManager, "AbilityManager");
        
        EditorGUILayout.LabelField("Buttons only work on play, do not use them in editor");

        if (GUILayout.Button("Give Dash"))
        {
            DataSerializer.Save(SaveDataKeywords.dashBought, true); 
            DataSerializer.Save(SaveDataKeywords.dashEquipped, true); 
            Debug.Log("Dash set to: " + DataSerializer.Load<bool>(SaveDataKeywords.dashEquipped));
        }
        
        if (GUILayout.Button("Remove Dash"))
        {
            DataSerializer.Save(SaveDataKeywords.dashBought, false); 
            DataSerializer.Save(SaveDataKeywords.dashEquipped, false); 
            Debug.Log("Dash set to: " + DataSerializer.Load<bool>(SaveDataKeywords.dashEquipped));
        }
        
        if (GUILayout.Button("Give Shield"))
        {
            DataSerializer.Save(SaveDataKeywords.shieldBought, true); 
            DataSerializer.Save(SaveDataKeywords.shieldEquipped, true); 
            Debug.Log("Dash set to: " + DataSerializer.Load<bool>(SaveDataKeywords.shieldEquipped));
        }
        
        if (GUILayout.Button("Remove Shield"))
        {
            DataSerializer.Save(SaveDataKeywords.shieldBought, false); 
            DataSerializer.Save(SaveDataKeywords.shieldEquipped, false); 
            Debug.Log("Dash set to: " + DataSerializer.Load<bool>(SaveDataKeywords.shieldEquipped));
        }
        
        if (GUILayout.Button("Set Health to Max"))
        {
            DataSerializer.Save(SaveDataKeywords.healthBought, true); 
            DataSerializer.Save(SaveDataKeywords.healthLevel, 3); 
            Debug.Log("Health set to: " + DataSerializer.Load<bool>(SaveDataKeywords.shieldEquipped));
        }
        
        if (GUILayout.Button("Set Health to Default"))
        {
            DataSerializer.Save(SaveDataKeywords.healthBought, false); 
            DataSerializer.Save(SaveDataKeywords.healthLevel, 0); 
            Debug.Log("Health set to: " + DataSerializer.Load<bool>(SaveDataKeywords.shieldEquipped));
        }
        
        if (GUILayout.Button("Set Default Weapon"))
        {
            DataSerializer.Save(SaveDataKeywords.burstBought, false); 
            DataSerializer.Save(SaveDataKeywords.tripleBought, false); 
            DataSerializer.Save(SaveDataKeywords.weaponEquiped, 0); 
            Debug.Log("Default Weapon");
        }
        
        if (GUILayout.Button("Set Burst Weapon"))
        {
            DataSerializer.Save(SaveDataKeywords.burstBought, true); 
            DataSerializer.Save(SaveDataKeywords.tripleBought, false); 
            DataSerializer.Save(SaveDataKeywords.weaponEquiped, 1); 
            Debug.Log("Burst Weapon");
        }
        
        if (GUILayout.Button("Set Triple Weapon"))
        {
            DataSerializer.Save(SaveDataKeywords.burstBought, false); 
            DataSerializer.Save(SaveDataKeywords.tripleBought, true); 
            DataSerializer.Save(SaveDataKeywords.weaponEquiped, 2); 
            Debug.Log("Burst Weapon");
        }
        
        DrawDefaultInspector();
    }
}
#endif

[System.Serializable]
public struct Ability
{
    public GameObject prefab;
    public bool isBought;
    public bool isEquipped;
}
