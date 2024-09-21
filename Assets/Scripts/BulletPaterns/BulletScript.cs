using System;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float bulletLife = 10.0f;  // Defines how long before the bullet is destroyed
    public float speed = 1.0f;


    private Vector2 spawnPoint;
    private float timer = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        spawnPoint = new Vector2(transform.position.x, transform.position.y);
    }


    // Update is called once per frame
    void Update()
    {
        if(timer > bulletLife) Destroy(this.gameObject);
        timer += Time.deltaTime;
        transform.position = Movement(timer);
    }

    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }

    private Vector2 Movement(float timer) {
        // Moves right according to the bullet's rotation
        float x = timer * speed * transform.right.x;
        float y = timer * speed * transform.right.y;
        return new Vector2(x+spawnPoint.x, y+spawnPoint.y);
    }
}
