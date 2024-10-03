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
    
    public EnemyWaypointsScriptableObject waypoints;

    private bool _arrivedAtLocation;

    private int _currentWaypointIndex = 0;

    private List<Waypoint> _waypointsList;

    private Transform _tr;
    
    // Start is called before the first frame update
    void Start()
    {
        _tr = this.transform;
        if (waypoints)
        {
            _waypointsList = waypoints.waypointsList;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (moveToPosition)
        {
            if (waypoints)
            {
                destination = _waypointsList[_currentWaypointIndex].WaypointPosition;
                if (_waypointsList[_currentWaypointIndex].DoOverride)
                {
                    this.speed = _waypointsList[_currentWaypointIndex].OverrideSpeed;
                    this.destroyAtArrival = _waypointsList[_currentWaypointIndex].OverrideDestroyAtArrival;
                    this.useLerp = _waypointsList[_currentWaypointIndex].OverrideUseLerp;
                }
            }
            
            {
                //Do not use Waypoints
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
                
            }
            
            float d = (destination.x - _tr.position.x) * (destination.x - _tr.position.x) +
                (destination.y - _tr.position.y) * (destination.y - _tr.position.y) - (0.1f);   //.1f offset
            if (d <= 0f)
            {
                if (waypoints)
                {
                    _currentWaypointIndex++;
                    if (_currentWaypointIndex > _waypointsList.Count - 1)
                    {
                        moveToPosition = false;
                        
                        if (destroyAtArrival)
                        {
                            Destroy(this.gameObject);
                        }
                    }
                }
                else
                {
                    //No waypoints Logic
                    moveToPosition = false;

                    if (destroyAtArrival)
                    {
                        Destroy(this.gameObject);
                    }
                }
            }

        }
    }
}
