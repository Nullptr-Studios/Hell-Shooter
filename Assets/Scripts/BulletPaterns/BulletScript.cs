using System;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float bulletLife = 10.0f;  // Defines how long before the bullet is destroyed
    public float speed = 1.0f;
    
    //Damage and multiplier is stored inside each bullet.
    public float storedDamage = 1.0f;
    public float storedMultiplier = 1.0f;

    public GameObject player;

    private Vector2 _spawnPoint;
    private float _timer = 0.0f;

    private Vector3 _transformRight;
    private Transform _tr;

    public void SetSpeed(float _speed)
    {
        this.speed = _speed;
    }

    public void SetLife(float life)
    {
        this.bulletLife = life;
    }

    public void SetLifeAndSpeed(float life, float s)
    {
        this.speed = s;
        this.bulletLife = life;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _tr = this.transform;
        _spawnPoint = new Vector2(transform.position.x, transform.position.y);
        _transformRight = _tr.right;
        
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if(_timer > bulletLife) 
            Destroy(this.gameObject);
        _timer += Time.fixedDeltaTime;
        _tr.position = Movement(_timer);
    }

    private Vector2 Movement(float timer) {
        //this is done, so we can avoid Unity expensive RigidBody
        //Moves right according to the bullet's rotation
        float x = timer * speed * _transformRight.x;
        float y = timer * speed * _transformRight.y;
        return new Vector2(x+_spawnPoint.x, y+_spawnPoint.y);
    }

    public void MyTriggerEnter()
    {
        //Damage multiplier is calculated here cuz fucking unity does not handle sending more than one param in SendMessage(), end my suffering pls -d
        float calculatedDamage = storedDamage * storedMultiplier;
        player.gameObject.SendMessage("DoDamage", calculatedDamage);
        Destroy(this.gameObject);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.unityLogger.Log(other.gameObject.name + " collided with " + this.gameObject.name);
        if (other.gameObject.CompareTag("Enemy") && CompareTag("PlayerBullet"))
        {
            //Damage multiplier is calculated here cuz fucking unity does not handle sending more than one param in SendMessage(), end my suffering pls -d
            float calculatedDamage = storedDamage * storedMultiplier;
            other.gameObject.SendMessage("DoDamage", calculatedDamage);
            Destroy(this.gameObject);
        }
        
        //Warning Asteroid implementation is on AsteroidController

        //@TODO: Add fancy effects
        
    }

}
