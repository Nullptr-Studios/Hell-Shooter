using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BulletSpawner : MonoBehaviour
{
    public enum SpawnerType { Straight, Spin, Circle }


    [Header("Bullet Attributes")]
    public GameObject bullet;
    /*public float bulletLife = 1.0f;
    public float speed = 1.0f;*/


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
                Fire();
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
                Fire();
            }
            
            timer = 0.0f;
        }
    }


    private void Fire() {
        if(bullet) {

            spawnedBullet = Instantiate(bullet, transform.position, Quaternion.identity);
            // let's try to avoid get component as much as we can, speed vars will be set in bullet prefab;
            //spawnedBullet.GetComponent<Bullet>().speed = speed;
            //spawnedBullet.GetComponent<Bullet>().bulletLife = bulletLife;
            spawnedBullet.transform.rotation = transform.rotation;
        }
    }
}