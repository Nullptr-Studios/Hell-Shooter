using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyWaypointsScriptableObject", menuName = "ScriptableObject/EnemyWaypointsScriptableObject")]
public partial class EnemyWaypointsScriptableObject : ScriptableObject
{
    public List<Vector2> waypointsList;
}
