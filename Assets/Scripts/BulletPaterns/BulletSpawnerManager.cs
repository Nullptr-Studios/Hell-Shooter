using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the Manager class of bullet spawners
/// This is the user front end editing for spawners
/// </summary>
public class BulletSpawnerManager : MonoBehaviour
{
    //This is a fucking mess I don't know were to fucking begin -d
    public enum PremadeShapeType { SimpleLine, SimpleSpin, SimpleCircle, SimpleSquare, SimpleDiamond, MultipleCircleRotating, Custom }
    
    public PremadeShapeType premadeShapeType;

    [Header("Spawner Configs")]
    public float firingRate = 0.25f;
    
    //This tells the spawner to spawn back and forth
    public bool backAndForth = false;

    [Header("Bullet Configs")] 
    public float bulletLife = 10.0f;
    public float bulletSpeed = 1.0f;
    
    [Header("Circle Configs")]
    public int bulletsPerShotInCircle = 20;
    public float rotatePerCircle = 1.5f;

    
    public bool usesSen = false;
    public float senTimeMultiplier = 1.0f;
    public float senAmplitude = 20.0f;
    
    [Header("Line Configs")]
    public float rotatePerShotLine = 1.5f;
    
    [Header("Complex Forms Configs")]
    public Vector2 localDistanceFromCenter = new Vector2(-1.5f, 1.5f);
    public bool overrideDistanceBetweenPoints = false;
    public float distanceBetweenSpawnPoints = 3.0f;
    public List<float> angleBetweenSpawnPoints = new List<float>();
    //@TODO: Refactor all these variables
    public float InitialAngle = 0.0f;

    public int amountOfSpawners = 0;

    public BulletSpawner.SpawnerType overrideComplexSpawnerType = BulletSpawner.SpawnerType.Straight;
    //Complex spawner handler transform
    public Transform complexSpawnPointTransform;
    
    
    public GameObject SpawnerPrefab;
    public List<GameObject> SpawnerList = new List<GameObject>();
    public List<BulletSpawner> SpawnerScriptList = new List<BulletSpawner>();

    [Header("Warning!! Do Not Press In Editor!!")]
    public bool refresh = false;
    

    private BulletSpawner.SpawnerType spawnerType;
    
    //Refresh Bool
    private FieldChangesTracker changesTracker = new FieldChangesTracker();

    private GameObject _player;

    private float _playerRadius;
    
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindWithTag("Player");
        _playerRadius = _player.GetComponent<PlayerBulletColision>().playerRadius;
        SelectSettingsForShape(premadeShapeType);
    }

    void SelectSettingsForShape(PremadeShapeType shapeType)
    {
        //Refresh all spawner list
        refresh = false;
        foreach (var spawner in SpawnerList)
        {
            Destroy(spawner);
        }
        
        SpawnerList.Clear();
        SpawnerScriptList.Clear();
        
        //Premade shapes
        switch (shapeType)
        {
            case PremadeShapeType.SimpleLine:
                amountOfSpawners = 1;
                spawnerType = BulletSpawner.SpawnerType.Straight;
                break;
            case PremadeShapeType.SimpleCircle:
                amountOfSpawners = 1;
                spawnerType = BulletSpawner.SpawnerType.Circle;
                break;
            case PremadeShapeType.SimpleSpin:
                amountOfSpawners = 1;
                spawnerType = BulletSpawner.SpawnerType.Spin;
                break;
            case PremadeShapeType.SimpleSquare:
                angleBetweenSpawnPoints.Clear();
                InitialAngle = 0.0f;
                amountOfSpawners = 4;
                distanceBetweenSpawnPoints = 3;
                angleBetweenSpawnPoints.Add(90.0f);
                localDistanceFromCenter = new Vector2(-1.5f, 1.5f);
                break;
            case PremadeShapeType.SimpleDiamond:
                angleBetweenSpawnPoints.Clear();
                amountOfSpawners = 4;
                distanceBetweenSpawnPoints = 3;
                InitialAngle = 60.0f;
                angleBetweenSpawnPoints.Add(120.0f);
                angleBetweenSpawnPoints.Add(60.0f);
                angleBetweenSpawnPoints.Add(120.0f);
                localDistanceFromCenter = new Vector2(1.0f, 1.5f);
                break;
            case PremadeShapeType.MultipleCircleRotating:

                break;
        }


        //Add required spawners to both lists
        if (amountOfSpawners > SpawnerList.Count)
        {
            int add = amountOfSpawners - SpawnerList.Count;
            for (int i = 0; i < add; i++)
            {
                GameObject spawner = Instantiate(SpawnerPrefab, this.gameObject.transform);
                BulletSpawner spawnerScript = spawner.GetComponent<BulletSpawner>();
                SpawnerList.Add(spawner);
                SpawnerScriptList.Add(spawnerScript);
            }
        }
        //This else if won't be called ever, since we clear the lists each refresh, however we'll keep it here in case something goes wrong :)
        else if (amountOfSpawners < SpawnerList.Count)
        {
            int remove = SpawnerList.Count - amountOfSpawners;
            for (int i = 0; i < remove; i++)
            {
                SpawnerList.RemoveAt(SpawnerList.Count);
                SpawnerScriptList.RemoveAt(SpawnerList.Count);
            }
        }

        //this index will be used to step in the angles list
        int index = 0;
        //reset complexSpawnPointTransform to it's default location and rotation (InitialAngle)
        complexSpawnPointTransform.localPosition = localDistanceFromCenter;
        complexSpawnPointTransform.transform.localEulerAngles = new Vector3(0f, 0f, -InitialAngle);
        
        //Yet again we step in each script to change settings, this can be done in the for above, but since this function is not called each frame we can afford having 2 for :)
        foreach (var spawner in SpawnerScriptList)
        {
            if (premadeShapeType == PremadeShapeType.SimpleSquare || premadeShapeType == PremadeShapeType.SimpleDiamond || premadeShapeType == PremadeShapeType.Custom)
            {
                //this is if the angle list is less than the spawner list
                int angleIndex = Mathf.Clamp(index, 0, angleBetweenSpawnPoints.Count - 1);
                /////////////////////////////////////////////////////////
                SpawnerList[index].transform.localPosition = complexSpawnPointTransform.localPosition;
                SpawnerList[index].transform.localRotation = complexSpawnPointTransform.localRotation;
                
                //Step determined units forward and rotate
                complexSpawnPointTransform.localPosition += complexSpawnPointTransform.right * distanceBetweenSpawnPoints;
                complexSpawnPointTransform.transform.localEulerAngles = new Vector3(0f,0f,complexSpawnPointTransform.transform.eulerAngles.z - angleBetweenSpawnPoints[angleIndex]);

                spawnerType = overrideComplexSpawnerType;
            }
            else
            {
                //if it's not a complex form, set position to local origin
                SpawnerList[index].transform.localPosition = new Vector3(0,0,0);
            }
            
            //Pass configs to spawner
            spawner.spawnerType = this.spawnerType;
            spawner.ammountOfBulletsInCircle = this.bulletsPerShotInCircle;
            spawner.rotateEachCircleCompletion = rotatePerCircle;
            spawner.rotatingEachBullet = rotatePerShotLine;

            spawner.firingRate = this.firingRate;
            spawner.bulletLife = this.bulletLife;
            spawner.speed = this.bulletSpeed;
            
            spawner.alsobackAndForth = backAndForth;

            spawner.usesSen = this.usesSen;
            spawner.senAmplitude = this.senAmplitude;
            spawner.senTimeMultiplier = this.senTimeMultiplier;

            spawner.player = this._player;
            spawner._pRadius = this._playerRadius;
            
            index++;
        }
    }
    
    /// <summary>
    /// OnValidate is called whenever some variable has changed in the class
    /// </summary>
    void OnValidate()
    {
        if (changesTracker.TrackFieldChanges(this, x => x.refresh))
            SelectSettingsForShape(premadeShapeType);
    }
    
}
