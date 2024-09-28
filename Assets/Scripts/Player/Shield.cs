using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shield : MonoBehaviour
{
    public SpriteRenderer shieldRenderer;
    public Collider2D shieldCollider;
    
    private PlayerInput _playerInput;
    
    // Start is called before the first frame update
    void Start()
    {
        _playerInput = transform.parent.GetComponent<PlayerInput>();
        _playerInput.onActionTriggered += OnShield;
        
        shieldRenderer.enabled = false;
        shieldCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnShield(InputAction.CallbackContext context)
    {
        if (context.action.name != "Shield")
            return;
        
        if (context.performed)
        {
            shieldRenderer.enabled = true;
            shieldCollider.enabled = true;
        }

        if (context.canceled)
        {
            shieldRenderer.enabled = false;
            shieldCollider.enabled = false;
        }
    }
}
