using ToolBox.Serialization;
using UnityEditor;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    // Abilities
    public Ability dash;
    public Ability shield;
    public Ability reinforcer;
    
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

        reinforcer.isBought = DataSerializer.Load<bool>(SaveDataKeywords.healthBought);
        reinforcer.isEquipped = DataSerializer.Load<bool>(SaveDataKeywords.healthBought); // The same because it cannot be unequipped once bought
        if (reinforcer.isBought)
            Instantiate(reinforcer.prefab, this.transform);
        
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
