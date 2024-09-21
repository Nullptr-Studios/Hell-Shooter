using UnityEngine;
using System;
using Unity.Collections;
using UnityEngine.InputSystem;

public class PlayerLookAt : MonoBehaviour
{
    [Range(0.0f,0.25f)] public float lerpMagnitude = 0.03f;

    public Vector3 dir = Vector3.zero;
    private Vector3 _currentDir = Vector3.zero;
    
    private PlayerInput playerInput;
    

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }
    
    //fuck unity
    public bool IsGamepad()
    {
        return playerInput.currentControlScheme.Equals("Gamepad") ? true : false;
    }
    
    private void OnLook(InputValue value)
    {
        //Check if we are recieving the input from a gamepad or mouse
        if (IsGamepad())
        {
            // since the joystik is not in world coordinates, we add them to the player coordinates and then convert them to screen coordinates
            dir = Camera.main.WorldToScreenPoint(new Vector3(value.Get<Vector2>().x, value.Get<Vector2>().y, 0.0f) 
                                                 + gameObject.transform.position) - Camera.main.WorldToScreenPoint(gameObject.transform.position);
            dir.Normalize();
        }
        else
        {
            //Obtain mouse dir 
            dir = new Vector3(value.Get<Vector2>().x,value.Get<Vector2>().y, 0.0f) - Camera.main.WorldToScreenPoint(gameObject.transform.position);
            dir.Normalize();
        }

    }
    
    void Update ()
    {
        //Lerp rotation over time
        _currentDir = Vector3.Slerp(_currentDir, dir, Time.deltaTime + lerpMagnitude); 
        float angle = Mathf.Atan2 (_currentDir.y, _currentDir.x) * Mathf.Rad2Deg;
        
        //Set rotation
        gameObject.transform.rotation = Quaternion.AngleAxis (angle - 90.0f, Vector3.forward);
    }
    
}
