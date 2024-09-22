using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawnerManager : MonoBehaviour
{
    
    public enum PremadeShapeType { SimpleLine, SimpleSpin, SimpleCircle, SimpleSquare, SimpleDiamond, MultipleCircleRotating, Custom }
    
    public PremadeShapeType premadeShapeType;

    public float firingRate = 0.25f;
    
    public bool backAndForth = false;

    [Header("Bullet Configs")] 
    public float bulletLife = 10.0f;
    public float bulletSpeed = 1.0f;
    
    [Header("Circle Configs")]
    public int bulletsPerShotInCircle = 20;
    
    public float rotatePerCircle = 1.5f;
    
    [Header("Line Configs")]
    public float rotatePerShotLine = 1.5f;
    
    [Header("Complex Forms Configs")]
    public Vector2 localDistanceFromCenter = new Vector2(-1.5f, 1.5f);
    public float distanceBetweenSpawnPoints = 3.0f;
    public List<float> angleBetweenSpawnPoints = new List<float>();
    public float InitialAngle = 0.0f;
    public BulletSpawner.SpawnerType overrideComplexSpawnerType = BulletSpawner.SpawnerType.Straight;
    public Transform complexSpawnPointTransform;
    
    
    public GameObject SpawnerPrefab;
    public List<GameObject> SpawnerList = new List<GameObject>();
    public List<BulletSpawner> SpawnerScriptList = new List<BulletSpawner>();
    
    public bool refresh = false;
    
    private int amountOfSpawners = 0;
    private BulletSpawner.SpawnerType spawnerType;
    
    //Refresh Bool
    private FieldChangesTracker changesTracker = new FieldChangesTracker();
    
    // Start is called before the first frame update
    void Start()
    {
        SelectSettingsForShape(premadeShapeType);
    }

    void SelectSettingsForShape(PremadeShapeType shapeType)
    {
        refresh = false;
        foreach (var spawner in SpawnerList)
        {
            Destroy(spawner);
        }
        
        SpawnerList.Clear();
        SpawnerScriptList.Clear();
        
        
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
        }else if (amountOfSpawners < SpawnerList.Count)
        {
            int remove = SpawnerList.Count - amountOfSpawners;
            for (int i = 0; i < remove; i++)
            {
                SpawnerList.RemoveAt(SpawnerList.Count);
                SpawnerScriptList.RemoveAt(SpawnerList.Count);
            }
        }

        int index = 0;
        complexSpawnPointTransform.localPosition = localDistanceFromCenter;
        complexSpawnPointTransform.rotation = new Quaternion(0, 0, 0, 0);
        complexSpawnPointTransform.transform.localEulerAngles = new Vector3(0f,0f,complexSpawnPointTransform.transform.eulerAngles.z - InitialAngle);
        
        foreach (var spawner in SpawnerScriptList)
        {

            if (premadeShapeType == PremadeShapeType.SimpleSquare || premadeShapeType == PremadeShapeType.SimpleDiamond || premadeShapeType == PremadeShapeType.Custom)
            {
                //this is if the angle list is less than the spawner list
                int angleIndex = Mathf.Clamp(index, 0, angleBetweenSpawnPoints.Count - 1);
                /////////////////////////////////////////////////////////
                SpawnerList[index].transform.localPosition = complexSpawnPointTransform.localPosition;
                SpawnerList[index].transform.localRotation = complexSpawnPointTransform.localRotation;
                
                complexSpawnPointTransform.localPosition += complexSpawnPointTransform.right * distanceBetweenSpawnPoints;
                complexSpawnPointTransform.transform.localEulerAngles = new Vector3(0f,0f,complexSpawnPointTransform.transform.eulerAngles.z - angleBetweenSpawnPoints[angleIndex]);
                spawnerType = overrideComplexSpawnerType;
            }
            else
            {
                SpawnerList[index].transform.localPosition = new Vector3(0,0,0);
            }
            
            
            spawner.spawnerType = this.spawnerType;
            spawner.firingRate = this.firingRate;
            spawner.ammountOfBulletsInCircle = this.bulletsPerShotInCircle;
            spawner.rotateEachCircleCompletion = rotatePerCircle;
            spawner.rotatingEachBullet = rotatePerShotLine;

            spawner.bulletLife = this.bulletLife;
            spawner.speed = this.bulletSpeed;
            
            spawner.AlsobackAndForth = backAndForth;
            
            index++;
        }
    }
    
    void OnValidate()
    {
        if (changesTracker.TrackFieldChanges(this, x => x.refresh))
            SelectSettingsForShape(premadeShapeType);
    }
    
}
