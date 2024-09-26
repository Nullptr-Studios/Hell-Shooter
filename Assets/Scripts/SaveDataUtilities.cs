using ToolBox.Serialization;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class SaveDataUtilities : MonoBehaviour
{
    
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