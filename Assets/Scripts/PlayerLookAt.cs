using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLookAt : MonoBehaviour
{
    [Range(0.0f,0.25f)] public float lerpMagnitude = 0.03f;
    
    private Vector3 _currentDir;
    void Update ()
    {
        //Obtain Mouse Dir
        Vector3 dir = Input.mousePosition - Camera.main.WorldToScreenPoint (gameObject.transform.position);
        dir.Normalize();
        
        //Lerp rotation over time
        _currentDir = Vector3.Slerp(_currentDir, dir, Time.deltaTime + lerpMagnitude); 
        float angle = Mathf.Atan2 (_currentDir.y, _currentDir.x) * Mathf.Rad2Deg;
        
        //Set rotation
        gameObject.transform.rotation = Quaternion.AngleAxis (angle - 90.0f, Vector3.forward);
        
    }
}
