using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyWaveScriptableObject", menuName = "ScriptableObject/EnemyWaveScriptableObject")]
public class EnemyWavesScriptableObject : ScriptableObject
{
     /*[SerializeField]*/ public List<EnemyWave> EnemyWavesList;

     public int _currentSteppingIndex = 0;
     private bool _started = false;
     private bool _isLast = false;
     private float _timer = 0.0f;

     private void OnDisable()
     {
         _currentSteppingIndex = 0;
         _started = false;
         _isLast = false;
         _timer = 0.0f;
     }

     // 👍 THIS FUNCTION IS ALL CORRECT 1:15 am
     //Spawning logic is inside Scriptable object to be more organized-d
     // This fucking caused me more pain while debugging xd -x
     public bool UpdateEnemyWave(float delta)
     {
         // Update timer
         _timer += delta;

         // Start wave
         if (!_started)
         {
             _started = true;
             SpawnWave(EnemyWavesList[_currentSteppingIndex]);
             _currentSteppingIndex++;
             if (_currentSteppingIndex > EnemyWavesList.Count-1) _isLast = true;
         }
         
         // Check if is last before if on line 40 because of array OutOfBounds
         if (_isLast)
         {
             _currentSteppingIndex = 0;
             _isLast = false;
             return true;
         }

         if (_timer >= EnemyWavesList[_currentSteppingIndex].delay)
         {
             _timer = 0;
             _started = false;
         }
         
         
         return false;
     }

     // 👍 THIS FUNCTION IS ALL CORRECT 0:20 am
     private void SpawnWave(EnemyWave Wave)
     {
         Debug.Log("Spawned: " + Wave.EnemyPrefab + " in position: " + Wave.SpawnLocation.ToString());
         // Instantiate enemy from prefab on position Wave.SpawnLocation
         GameObject Enemy = Instantiate(Wave.EnemyPrefab,
             new Vector3(Wave.SpawnLocation.x, Wave.SpawnLocation.y, 0), 
             new Quaternion());
         // Gets the EnemyMovement component
         var movement = Enemy.GetComponent<EnemyMovement>();

         if (Wave.overrideHealth > 0)
         {
             var health = Enemy.GetComponent<EnemyHealthSystem>();
             health.ChangeMaxHealth(Wave.overrideHealth);
         }

         // Sets destination
         movement.destination = Wave.DestinationLocation;
         // Set use lerp
         movement.useLerp = Wave.useLerp;
         // Set movement speed 
         movement.speed = Wave.speed;
         movement.destroyAtArrival = Wave.DestroyOnArrival;
         // Set move to position if speed is > 0
         if (Wave.speed > 0)
         {
             movement.moveToPosition = true;
         }
         movement.bulletSpawner.transform.localEulerAngles = new Vector3(0,0, Wave.spawnerAngle);
     }
}

[System.Serializable]
public struct EnemyWave
{
    public GameObject EnemyPrefab;
    public int ammount; //Lets stick to 1 enemy rn, TODO: Come back to this later
    //Delay between waves
    public float delay;
    public Vector2 SpawnLocation;
    //@TODO: Add support for enemies
    public Vector2 DestinationLocation;
    public bool DestroyOnArrival;
    public float overrideHealth;
    public bool useLerp;
    public float speed;
    public float spawnerAngle;
    public EnemyWaypointsScriptableObject WaypointsSo;
    //@TODO: Add Follow waypoints
}

// Timer on here also didn't work properly
// It checked the delay of the previous one rather to its own
// To be done more clearly, we should make an Async wait function on an Interface Class