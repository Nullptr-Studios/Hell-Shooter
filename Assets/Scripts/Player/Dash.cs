using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Dash : MonoBehaviour
{
    [NonSerialized] public bool isDashActive;
    [NonSerialized] public bool canDash = true;
    public float speedMultiplier;
    public float duration;
    public float cooldown;
    private float startedDash;
    private float startedCooldown;
    public bool disableMovementOnDash = true;
    
    [Header("Graphics")]
    public TrailRenderer trailRenderer;

    private PlayerMovement _movement;
    private PlayerLookAt _lookAt;
    
    #if UNITY_EDITOR
    [Header("Debug")]
    public bool log;
    #endif

    void Awake()
    {
        _movement = transform.parent.GetComponent<PlayerMovement>();
        _lookAt = transform.parent.GetComponent<PlayerLookAt>();
        
        trailRenderer.enabled = false;
        trailRenderer.time = duration * 2;
    }

    void Update()
    {
        // End dash logic
        if (isDashActive && Time.time - startedDash >= duration)
        {   
            isDashActive = false;
            _movement.maxSpeed /= speedMultiplier;
            _movement.dir = Vector2.zero;
            canDash = false;
            startedCooldown = Time.time;
            
            trailRenderer.enabled = false;
            
            #if UNITY_EDITOR
            if (log) Debug.Log("Dash cooldown started");
            #endif
        }
        
        // Stops cooldown
        if (!canDash && Time.time - startedDash >= cooldown)
        {
            canDash = true;
            #if UNITY_EDITOR
            if (log) Debug.Log("Dash cooldown ended");
            #endif
        }
    }
    
    /// <summary>
    /// Dash logic when button is pressed.
    /// Called by PlayerInput component. Need to be in BroadcastMessage mode.
    /// </summary>
    private void OnDash()
    {
        if (!canDash) 
            return;
        
        isDashActive = true;
        startedDash = Time.time;
        _movement.maxSpeed *= speedMultiplier;
        trailRenderer.enabled = true;
        
        #if UNITY_EDITOR
        if (log) Debug.Log("Dash");
        #endif
    }
}
