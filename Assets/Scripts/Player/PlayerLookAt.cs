using UnityEngine;
using System;
using Unity.Collections;
using UnityEngine.InputSystem;

public class PlayerLookAt : MonoBehaviour
{
    public float lerpMagnitude = 0.03f;

    private Vector3 dir = Vector3.zero;
    private Vector3 _currentDir = Vector3.zero;
    
    private PlayerInput _playerInput;
    private LevelMenu _levelMenu; // This is needed to disable rotation on Level Menu
    private bool _menuNullCheck;
    
    private bool UsingMouse = false;
    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        
        var _levelMenuObject = GameObject.Find("LevelMenu");
        if (_levelMenuObject != null)
        {
            _levelMenu = GameObject.Find("LevelMenu").GetComponent<LevelMenu>();
        }
        
        _menuNullCheck = _levelMenuObject == null;
        
        _playerInput.onActionTriggered += OnLook;
    }
    
    //fuck unity
    public bool IsGamepad()
    {
        return _playerInput.currentControlScheme.Equals("Gamepad") ? true : false;
    }
    
    private void OnLook(InputAction.CallbackContext context)
    {
        if (context.action.name != "Look")
            return;
        
        //Check if we are receiving the input from a gamepad or mouse
        if (IsGamepad())
        {
            UsingMouse = false; 
            // since the joystick is not in world coordinates, we add them to the player coordinates and then convert them to screen coordinates
            dir = Camera.main.WorldToScreenPoint(new Vector3(context.ReadValue<Vector2>().x, context.ReadValue<Vector2>().y, 0.0f) 
                                                 + gameObject.transform.position) - Camera.main.WorldToScreenPoint(gameObject.transform.position);
            dir.Normalize();
        }
        else
        {
            UsingMouse = true;
            //Obtain mouse dir 
            //dir = new Vector3(value.Get<Vector2>().x,value.Get<Vector2>().y, 0.0f) - Camera.main.WorldToScreenPoint(gameObject.transform.position);
            //dir.Normalize();
        }

    }
    
    void Update ()
    {
        if (!_menuNullCheck && _levelMenu.isMenuOpen)
            return;
        
        if (UsingMouse)
        {
            //Obtain mouse dir 
            dir = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.0f) -
                  Camera.main.WorldToScreenPoint(gameObject.transform.position);
            dir.Normalize();
        }

        //Lerp rotation over time
        _currentDir = Vector3.Slerp(_currentDir, dir, Time.deltaTime * lerpMagnitude); 
        float angle = Mathf.Atan2 (_currentDir.y, _currentDir.x) * Mathf.Rad2Deg;
        
        //Set rotation
        gameObject.transform.rotation = Quaternion.AngleAxis (angle - 90.0f, Vector3.forward);
        
        
    }
    
}
