using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyWaveScriptableObject", menuName = "ScriptableObject/EnemyWaveScriptableObject")]
public class EnemyWavesScriptableObject : ScriptableObject
{
     [SerializeField] public List<EnemyWave> EnemyWavesList;

     private int _currentSteppingIndex = 0;

     private bool _started = false;

     private float _timer = 0.0f;

     public bool UpdateEnemyWave(float delta)
     {
         _timer += delta;
         if (!_started)
         {
             _started = true;
             SpawnWave(EnemyWavesList[_currentSteppingIndex]);
         }

         if (_timer >= EnemyWavesList[_currentSteppingIndex].delay)
         {
             _currentSteppingIndex++;
             _timer = 0;
             _started = false;
             if (_currentSteppingIndex > EnemyWavesList.Count - 1)
             {
                 //wave done
                 _currentSteppingIndex = 0;
                 return true;
             }
         }
         
         return false;
     }

     private void SpawnWave(EnemyWave Wave)
     {
         Debug.Log("Spawned: " + Wave.EnemyPrefab.ToString());
         for (int i = 0; i < Wave.ammount; i++)
         {
             GameObject Enemy = Instantiate(Wave.EnemyPrefab,
                 new Vector3(Wave.SpawnLocation.x, Wave.SpawnLocation.y, 0), new Quaternion());
             //@TODO: Pass enemy data
         }
     }
}

[System.Serializable]
public struct EnemyWave
{
    public GameObject EnemyPrefab;
    public int ammount;
    public float delay;
    public Vector2 SpawnLocation;
    public Vector2 DestinationLocation;
    public EnemyWaypointsScriptableObject WaypointsSo;
    //@TODO: Add Follow waypoints
}
