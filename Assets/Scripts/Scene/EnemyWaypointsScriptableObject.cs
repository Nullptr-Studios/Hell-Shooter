using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyWaypointsScriptableObject", menuName = "ScriptableObject/EnemyWaypointsScriptableObject")]
public class EnemyWaypointsScriptableObject : ScriptableObject
{
    public List<Vector2> waypointsList;
}
