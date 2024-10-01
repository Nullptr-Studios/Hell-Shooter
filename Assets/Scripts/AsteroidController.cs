using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AsteroidController : MonoBehaviour
{
    public int currentAsteroidHealth = 10;
    public float velMagnitude = 10;
    public bool randomVel = true;
    public Vector2 asteroidStartingVel = new Vector2(-1,-1);
    public GameObject asteroidPrefab;

    public List<Sprite> SpriteList = new List<Sprite>();

    private GameObject player;
    private Rigidbody2D rb;


    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindWithTag("Player");
        rb = this.GetComponent<Rigidbody2D>();

        if (randomVel)
        {
            //fuck random it dosent work
            /*float x = UnityEngine.Random.Range(-1, 1);
            float y = UnityEngine.Random.Range(-1, 1);*/



            asteroidStartingVel *= UnityEngine.Random.Range(1, 3);
        }

        rb.velocity = asteroidStartingVel;
        rb.AddTorque(UnityEngine.Random.Range(-20, 20));

        float scale = UnityEngine.Random.Range(.5f, 1);

        transform.localScale = new Vector3(scale, scale, scale);

        if (SpriteList.Count != 0)
        {
            GetComponent<SpriteRenderer>().sprite = SpriteList[UnityEngine.Random.Range(0, SpriteList.Count - 1)];
        }

    }

    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerBullet"))
        {
            if (currentAsteroidHealth <= 0)
            {
                //Destroy
                player.SendMessage("GiveXP", 5);

                Destroy(this.gameObject);
                Destroy(collision.gameObject);
                currentAsteroidHealth = 0;
            }
            else
            {
                currentAsteroidHealth--;
                Destroy(collision.gameObject);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            other.gameObject.SendMessage("DoDamage",1);
            Destroy(this.gameObject);
        }
    }
}
