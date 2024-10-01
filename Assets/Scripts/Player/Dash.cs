using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Dash : MonoBehaviour
{
    public float speedMultiplier;
    public float duration;
    public float cooldown;
    [Tooltip("Threshold to disable dash when player is not moving")]
    private const float DASH_THRESHOLD = 0.15f;
    public bool disableMovementOnDash = true;
    
    [NonSerialized] public bool isDashActive;
    [NonSerialized] public bool canDash = true;
    [NonSerialized] public Vector2 direction;
    [NonSerialized] public float speed;
    private float startedDash;
    private float startedCooldown;

    /// <summary>
    /// Returns a value from 0 to 1 that indicates how much time is left until the ability is not on cooldown
    /// </summary>
    public float cProgress;
    
    [Header("Graphics")]
    public TrailRenderer trailRenderer;
    
    [Header("Audio")]
    public AudioSource audioSource;
    
    public AudioClip dashPerformedSound;
    public AudioClip dashDeniedSound;
    public AudioClip cooldownFinalizedSound;

    private PlayerInput _playerInput;
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
        speed = _movement.maxSpeed * speedMultiplier;
        
        _playerInput = transform.parent.GetComponent<PlayerInput>();
        _playerInput.onActionTriggered += OnDash;
        
        trailRenderer.enabled = false;
        trailRenderer.time = duration * 2;
    }

    void Update()
    {
        // End dash logic
        if (isDashActive && Time.time - startedDash >= duration)
        {   
            isDashActive = false;
            canDash = false;
            startedCooldown = Time.time;
            
            trailRenderer.enabled = false;
            
#if UNITY_EDITOR
            if (log) Debug.Log("Dash cooldown started");
#endif
            
        }

        if (Time.time < cooldown && startedCooldown == 0)
            cProgress = 1;
        else
            cProgress = Mathf.Clamp01((Time.time - startedCooldown)/cooldown);
        
        // Stops cooldown
        if (!canDash && Time.time - startedDash >= cooldown)
        {
            canDash = true;
            
            //Sound
            if (audioSource)
            {
                audioSource.clip = cooldownFinalizedSound;
                audioSource.Play();
            }
            
#if UNITY_EDITOR
            if (log) Debug.Log("Dash cooldown ended");
#endif
            
        }
    }
    
    /// <summary>
    /// Dash logic when button is pressed.
    /// Called by PlayerInput component. Need to be in BroadcastMessage mode.
    /// </summary>
    private void OnDash(InputAction.CallbackContext context)
    {
        if (context.action.name != "Dash")
            return;

        if (!context.performed)
            return;

        if (!canDash)
        {
            //Sound
            if (audioSource)
            {
                audioSource.clip = dashDeniedSound;
                audioSource.Play();
            }

            return;
        }
            
        if (_movement.dir.magnitude < DASH_THRESHOLD)
            return;
        
        isDashActive = true;
        startedDash = Time.time;
        direction = _movement.dir;
        trailRenderer.enabled = true;
        
        //Sound
        if (audioSource)
        {
            audioSource.clip = dashPerformedSound;
            audioSource.Play();
        }
        
#if UNITY_EDITOR
        if (log) Debug.Log("Dash");
#endif
        
    }
}
