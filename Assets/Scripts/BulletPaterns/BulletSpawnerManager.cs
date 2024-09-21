using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawnerManager : MonoBehaviour
{
    
    public enum PremadeShapeType { SimpleLine, SimpleSpin, SimpleCircle, SimpleSquare, SimpleDiamond, MultipleCircleRotating }
    
    public PremadeShapeType premadeShapeType;

    public float firingRate = 0.25f;
    
    public int bulletsPerShotInCircle = 20;
    
    public float rotatePerCircle = 1.5f;
    
    public float rotatePerShotLine = 1.5f;
    
    public GameObject SpawnerPrefab;
    public List<GameObject> SpawnerList = new List<GameObject>();
    public List<BulletSpawner> SpawnerScriptList = new List<BulletSpawner>();
    
    public bool refresh = false;
    
    private int amountOfSpawners = 0;
    private BulletSpawner.SpawnerType spawnerType;
    
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
                
                break;
            case PremadeShapeType.SimpleDiamond:
                
                break;
            case PremadeShapeType.MultipleCircleRotating:
                
                break;
        }

        if (amountOfSpawners > SpawnerList.Count)
        {
            int add = amountOfSpawners - SpawnerList.Count;
            for (int i = 0; i < add; i++)
            {
                GameObject spawner = Instantiate(SpawnerPrefab);
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

        foreach (var spawner in SpawnerScriptList)
        {
            spawner.spawnerType = this.spawnerType;
            spawner.firingRate = this.firingRate;
            spawner.ammountOfBulletsInCircle = this.bulletsPerShotInCircle;
            spawner.rotateEachCircleCompletion = rotatePerCircle;
            spawner.rotatingEachBullet = rotatePerShotLine;
        }
    }
    
    void OnValidate()
    {
        if (changesTracker.TrackFieldChanges(this, x => x.refresh))
            SelectSettingsForShape(premadeShapeType);
    }
    
}
