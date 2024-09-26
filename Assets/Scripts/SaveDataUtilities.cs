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
    public const string goldCoins = "goldCoins";
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