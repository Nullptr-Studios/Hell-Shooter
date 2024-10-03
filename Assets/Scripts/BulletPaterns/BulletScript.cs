using System;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float bulletLife = 10.0f;  // Defines how long before the bullet is destroyed
    public float speed = 1.0f;
    
    //Damage and multiplier is stored inside each bullet.
    public float storedDamage = 1.0f;

    public GameObject PlayerHitEffectPrefab;

    private Vector2 _spawnPoint;
    private float _timer = 0.0f;

    private Vector2 _direction;
    private Transform _tr;

    public void SetLifeAndSpeed(float life, float s)
    {
        this.speed = s;
        this.bulletLife = life;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _tr = transform;  // Cache the transform reference
        _spawnPoint = _tr.position;  // Use Vector2 directly for more efficient storage
        _direction = _tr.right.normalized;  // Cache direction to avoid recomputing per frame

    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }


    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer > bulletLife)
        {
            Destroy(gameObject);  // Slightly faster than `this.gameObject`
            return;  // Early return to avoid unnecessary movement calculations
        }
        
        // Only move if the bullet is alive
        _tr.position = _spawnPoint + _direction * (_timer * speed);  // Avoid calling the Movement() method
    }

    /*private Vector2 Movement(float timer) {
        //this is done, so we can avoid Unity expensive RigidBody
        //Moves right according to the bullet's rotation
        float x = timer * speed * _transformRight.x;
        float y = timer * speed * _transformRight.y;
        return new Vector2(x+_spawnPoint.x, y+_spawnPoint.y);
    }*/
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Debug.unityLogger.Log(other.gameObject.name + " collided with " + this.gameObject.name);
        if (other.gameObject.CompareTag("Enemy") && CompareTag("PlayerBullet"))
        {
            other.gameObject.SendMessage("DoDamage", storedDamage);

            Instantiate(PlayerHitEffectPrefab, transform.position, new Quaternion());
            
            Destroy(this.gameObject);
            return;
        }else if (other.gameObject.CompareTag("Asteroid"))
        {
            Instantiate(PlayerHitEffectPrefab, transform.position, new Quaternion());
        }
        
        //Warning! Asteroid implementation is on AsteroidController
        
    }

}
