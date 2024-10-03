using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicEnemyMovement : MonoBehaviour
{

    public GameObject bulletSpawner;

    public float speed = 5.0f;

    public bool gotoPlayer;
    public bool lookAtPlayer;

    private Transform _tr;
    private Rigidbody2D _rb;

    private GameObject _p;

    private Vector2 _enemyToPlayer;
    
    // Start is called before the first frame update
    void Start()
    {
        _tr = transform;
        _rb = GetComponent<Rigidbody2D>();
        _p = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (_p)
        {
            _enemyToPlayer = _p.transform.position - gameObject.transform.position;
            _enemyToPlayer.Normalize();

            if (gotoPlayer)
            {
                _rb.velocity += _enemyToPlayer * (speed * Time.deltaTime);
            }

            if (lookAtPlayer)
            {
                float rot_z = Mathf.Atan2(_enemyToPlayer.y, _enemyToPlayer.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0f, 0f, rot_z + 90);
            }
        }
        else
        {
            Destroy(this);
        }
    }
}
