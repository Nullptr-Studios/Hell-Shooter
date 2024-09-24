using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public bool moveToPosition;
    public Vector2 destination;
    public float speed;
    
    private Rigidbody2D rb;
    
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (moveToPosition)
        {
            var step = speed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, destination, step);
            if (transform.position == new Vector3(destination.x, destination.y, 0))
                moveToPosition = false;
        }
    }
}
