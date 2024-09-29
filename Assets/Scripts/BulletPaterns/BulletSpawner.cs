using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


public class BulletSpawner : MonoBehaviour
{
    public enum SpawnerType { Straight, Spin, Circle }

    public bool alsobackAndForth = false;

    [Header("Bullet Attributes")]
    public GameObject bullet;
    public float bulletLife = 10.0f;
    public float speed = 1.0f;


    [Header("Spawner Attributes")]
    [SerializeField] public SpawnerType spawnerType;
    [SerializeField] public float firingRate = 1.0f;

    
    [Header("Spin Attributes")]
    [SerializeField] public float rotatingEachBullet = 5.0f;
    
    [Header("Circle Attributes")]
    [SerializeField] public int ammountOfBulletsInCircle = 5;
    [SerializeField] public float rotateEachCircleCompletion = 15.0f;
    public bool usesSen = false;
    public float senTimeMultiplier = 1.0f;
    public float senAmplitude = 20.0f;

    public GameObject player;

    private GameObject _spawnedBullet;
    private float _timer = 0.0f;
    private float _senTimer = 0.0f;

    public PlayerBulletColision _pCollider;
    private void Start()
    {
        bullet.GetComponent<EnemyBulletColision>().player = this.player;
        bullet.GetComponent<BulletScript>().player = this.player;
        bullet.GetComponent<EnemyBulletColision>().playerCollider = this._pCollider;
    }

    // Update is called once per frame
    void Update()
    {
        float delta = Time.deltaTime;
        _timer += delta;
        _senTimer += delta;

        if (_senTimer >= 180.0f)
        {
            _senTimer -= 180.0f;
        }
        
        if(_timer >= firingRate) {

            if (spawnerType == SpawnerType.Spin)
            {
                if (usesSen)
                {
                    transform.eulerAngles = new Vector3(0f, 0f, transform.eulerAngles.z + MathF.Sin(_senTimer*senTimeMultiplier)*senAmplitude);
                }
                else
                {
                    transform.eulerAngles = new Vector3(0f, 0f, transform.eulerAngles.z + rotatingEachBullet);
                }
                FireLogic();
            }
            else if (spawnerType == SpawnerType.Circle)
            {
                float angleEachStep = 360.0f / ammountOfBulletsInCircle;
                for (int i = 0; i < ammountOfBulletsInCircle; i++)
                {
                    Fire();
                    transform.eulerAngles = new Vector3(0f,0f,transform.eulerAngles.z + angleEachStep);
                }

                if (usesSen)
                {
                    transform.eulerAngles = new Vector3(0f, 0f, transform.eulerAngles.z + MathF.Sin(_senTimer*senTimeMultiplier)*senAmplitude);
                }
                else
                {
                    transform.eulerAngles = new Vector3(0f,0f,transform.eulerAngles.z + rotateEachCircleCompletion);
                }
            }
            else if (spawnerType == SpawnerType.Straight)
            {
                FireLogic();
            }
            
            _timer = 0.0f;
        }
    }

    private void FireLogic()
    {
        if (alsobackAndForth && spawnerType != SpawnerType.Circle)
        {
            Fire();
            transform.eulerAngles = new Vector3(0f, 0f, transform.eulerAngles.z + 180.0f);
            Fire();
            transform.eulerAngles = new Vector3(0f, 0f, transform.eulerAngles.z - 180.0f);
        }
        else
        {
            Fire();
        }
    }


    private void Fire() {
        if(bullet) {

            _spawnedBullet = Instantiate(bullet, transform.position, Quaternion.identity);

            _spawnedBullet.GetComponent<BulletScript>().SetLifeAndSpeed(bulletLife, speed);

            /*_spawnedBullet.GetComponent<BulletScript>().speed = speed;
            _spawnedBullet.GetComponent<BulletScript>().bulletLife = bulletLife;*/

            /*_spawnedBullet.SendMessage("SetSpeed", speed);
            _spawnedBullet.SendMessage("SetLife", bulletLife);*/

            _spawnedBullet.transform.rotation = transform.rotation;
        }
    }
}