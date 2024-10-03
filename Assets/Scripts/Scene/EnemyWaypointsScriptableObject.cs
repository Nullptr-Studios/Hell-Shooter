using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyWaypointsScriptableObject", menuName = "ScriptableObject/EnemyWaypointsScriptableObject")]
public partial class EnemyWaypointsScriptableObject : ScriptableObject
{
    public List<Waypoint> waypointsList;
}

[Serializable]
public struct Waypoint
{
    public Vector2 WaypointPosition;
    public bool DoOverride;
    public bool OverrideUseLerp;
    public bool OverrideDestroyAtArrival;
    public float OverrideSpeed;
}
