using System;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float bulletLife = 10.0f;  // Defines how long before the bullet is destroyed
    public float speed = 1.0f;
    
    public float storedDamage = 1.0f;
    public float storedMultiplier = 1.0f;

    private Vector2 spawnPoint;
    private float _timer = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        spawnPoint = new Vector2(transform.position.x, transform.position.y);
    }


    // Update is called once per frame
    void Update()
    {
        if(_timer > bulletLife) Destroy(this.gameObject);
        _timer += Time.deltaTime;
        transform.position = Movement(_timer);
    }

    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }

    private Vector2 Movement(float timer) {
        //this is done so we can avoid Unity expensive RigidBody
        // Moves right according to the bullet's rotation
        float x = timer * speed * transform.right.x;
        float y = timer * speed * transform.right.y;
        return new Vector2(x+spawnPoint.x, y+spawnPoint.y);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.unityLogger.Log(other.gameObject.name + " collided with " + this.gameObject.name);
        if (other.gameObject.CompareTag("Player") && CompareTag("EnemyBullet") || other.gameObject.CompareTag("Enemy") && CompareTag("PlayerBullet"))
        {
            //Damage multiplier is calculated here cuz fucking unity does not handle sending more than one param in SendMessage(), end my suffering pls -d
            float calculatedDamage = storedDamage * storedMultiplier;
            other.gameObject.SendMessage("DoDamage", calculatedDamage);
        }
    }

}
