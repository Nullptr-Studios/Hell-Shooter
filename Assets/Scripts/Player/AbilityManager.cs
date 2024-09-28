using ToolBox.Serialization;
using UnityEditor;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    // Abilities
    public Ability dash;
    public Ability shield;
    
    // Weapons
    public Ability defaultWeapon;
    public Ability burstWeapon;
    public Ability aimbotWeapon;
    
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
        
        
        burstWeapon.isBought = DataSerializer.Load<bool>(SaveDataKeywords.burstBought);
        burstWeapon.isEquipped = DataSerializer.Load<bool>(SaveDataKeywords.burstEquipped);
        aimbotWeapon.isBought = DataSerializer.Load<bool>(SaveDataKeywords.aimBought);
        aimbotWeapon.isEquipped = DataSerializer.Load<bool>(SaveDataKeywords.aimEquipped);
        if (burstWeapon.isBought && burstWeapon.isEquipped)
            Instantiate(burstWeapon.prefab, this.transform);
        else if (aimbotWeapon.isBought && aimbotWeapon.isEquipped)
            Instantiate(aimbotWeapon.prefab, this.transform);
        else
            Instantiate(defaultWeapon.prefab, this.transform);
    }

    
}

#if UNITY_EDITOR
[CustomEditor(typeof(AbilityManager))]
class AbilityManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var abilityManager = (AbilityManager)target;
        if (abilityManager == null) return;
        
        Undo.RecordObject(abilityManager, "AbilityManager");

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
