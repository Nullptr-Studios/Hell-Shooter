using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BulletSpawner : MonoBehaviour
{
    public enum SpawnerType { Straight, Spin, Circle }

    public bool AlsobackAndForth = false;

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


    private GameObject spawnedBullet;
    private float timer = 0.0f;


    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        
        if(timer >= firingRate) {

            if (spawnerType == SpawnerType.Spin)
            {
                transform.eulerAngles = new Vector3(0f,0f,transform.eulerAngles.z + rotatingEachBullet);
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
                transform.eulerAngles = new Vector3(0f,0f,transform.eulerAngles.z + rotateEachCircleCompletion);
            }
            else if (spawnerType == SpawnerType.Straight)
            {
                FireLogic();
            }
            
            timer = 0.0f;
        }
    }

    private void FireLogic()
    {
        if (AlsobackAndForth)
        {
            Fire();
            transform.eulerAngles = new Vector3(0f, 0f, transform.eulerAngles.z + 180.0f);
            Fire();
            transform.eulerAngles = new Vector3(0f, 0f, transform.eulerAngles.z + 180.0f);
        }
        else
        {
            Fire();
        }
    }


    private void Fire() {
        if(bullet) {

            spawnedBullet = Instantiate(bullet, transform.position, Quaternion.identity);

            spawnedBullet.GetComponent<BulletScript>().speed = speed;
            spawnedBullet.GetComponent<BulletScript>().bulletLife = bulletLife;

            spawnedBullet.transform.rotation = transform.rotation;
        }
    }
}