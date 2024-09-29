using UnityEngine;
using UnityEngine.InputSystem;

public class Shield : MonoBehaviour
{
    public int maxHealth = 1;
    public float cooldownTime;
    
    private int _currentHealth;
    private float _cooldownStart;
    private bool _onCooldown;
    private bool _shieldActive;
    
    // This is here in case it's needed later on
    //
    // /// <summary>
    // /// Current health the shield has.
    // /// Read only.
    // /// </summary>
    // public int health
    // {
    //     get => _currentHealth; 
    // }
    //
    // /// <summary>
    // /// Returns true if Shield cooldown is active and cannot be used.
    // /// Read only.
    // /// </summary>
    // public bool cooldownActive
    // {
    //     get => _onCooldown;
    // }

    /// <summary>
    /// Returns true if Shield is active at the moment.
    /// Read only.
    /// </summary>
    public bool isActive
    {
        get => _shieldActive;
    }
    
    private SpriteRenderer _renderer;
    private Collider2D _collider;
    private PlayerBulletColision _customCollider;
    
    private PlayerInput _playerInput;

#if UNITY_EDITOR
    [Header("Debug")]
    [SerializeField] private bool logCooldown;
#endif
    
    // Start is called before the first frame update
    void Start()
    {
        _playerInput = transform.parent.GetComponent<PlayerInput>();
        _playerInput.onActionTriggered += OnShield;
        
        _renderer = GetComponent<SpriteRenderer>();
        _renderer.enabled = false;
        _collider = GetComponent<Collider2D>();
        _collider.enabled = false;
        _customCollider = transform.parent.GetComponent<PlayerBulletColision>();
    }

    // Update is called once per frame
    void Update()
    {
        // Cooldown timer end check
        if (Time.time - _cooldownStart > cooldownTime)
        {
            _currentHealth = maxHealth;
            _onCooldown = false;
            
#if UNITY_EDITOR
            if (logCooldown) Debug.Log("Shield cooldown ended");
#endif
            
        }
    }

    /// <summary>
    /// Removes one health point from the shield and checks for low health
    /// </summary>
    public void DoDamage()
    {
        _currentHealth--;
        
        // Low health check
        if (_currentHealth <= 0)
        {
            _cooldownStart = Time.time;
            _onCooldown = true;
            _shieldActive = false;
            _customCollider.playerRadius = _customCollider.defaultRadius;

#if UNITY_EDITOR
            if (logCooldown) Debug.Log("Shield cooldown started");
#endif
            
            // Here add the particles and sound when needed
            _renderer.enabled = false;
            _collider.enabled = false;
        }
    }

    /// <summary>
    /// Shield logic.
    /// Invoked by Player Controller component using C# Events
    /// </summary>
    /// <param name="context">Input Action Context</param>
    public void OnShield(InputAction.CallbackContext context)
    {
        if (context.action.name != "Shield")
            return;
        
        if (_onCooldown)
            return;
        
        if (context.performed)
        {
            _shieldActive = true;
            _customCollider.playerRadius = 0.7f;
            
            _renderer.enabled = true;
            _collider.enabled = true;
        }

        if (context.canceled)
        {
            _shieldActive = false;
            _customCollider.playerRadius = _customCollider.defaultRadius;
            
            _renderer.enabled = false;
            _collider.enabled = false;
        }
    }
}
