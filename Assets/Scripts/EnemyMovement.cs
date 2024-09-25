using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public bool moveToPosition;
    public Vector2 destination;
    public float speed;
    public bool useLerp = true;

    public GameObject bulletSpawner;

    public bool destroyAtArrival = false;

    private Transform _tr;
    
    // Start is called before the first frame update
    void Start()
    {
        _tr = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (moveToPosition)
        {
            if (!useLerp)
            {
                float step = speed * Time.deltaTime; // calculate distance to move
                _tr.position = Vector3.MoveTowards(_tr.position, destination, step);
            }
            else
            {
                //lerp interpolates between 2 vectors, so iths the same result as the function above except this time is interpolated.
                _tr.position = Vector3.Lerp(_tr.position, destination, Time.deltaTime * speed);
            }

            float d = (destination.x - _tr.position.x) * (destination.x - _tr.position.x) +
                (destination.y - _tr.position.y) * (destination.y - _tr.position.y) - (0.1f);
            if (d <= 0f)
            {
                moveToPosition = false;

                if (destroyAtArrival)
                {
                    Destroy(this.gameObject);
                }
            }
                
        }
    }
}
