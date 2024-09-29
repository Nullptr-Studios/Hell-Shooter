using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BulletSpawnerManager;


[CreateAssetMenu(fileName = "SpawnerScriptableObjects", menuName = "ScriptableObject/SpawnerScriptableObjects")]
public class SpawnerScriptableObjects : ScriptableObject
{

    public PremadeShapeType premadeShapeType;

    [Header("Spawner Configs")]
    public float firingRate = 0.25f;

    //This tells the spawner to spawn back and forth
    public bool backAndForth = false;

    [Header("Bullet Configs")]
    public float bulletLife = 30.0f;
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

    public float InitialAngle = 0.0f;

    public int amountOfSpawners = 0;

    public BulletSpawner.SpawnerType overrideComplexSpawnerType = BulletSpawner.SpawnerType.Straight;
}
