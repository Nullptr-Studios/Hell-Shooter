using UnityEngine;
using UnityEngine.InputSystem;

public class Shield : MonoBehaviour
{
    public int maxHealth = 1;
    private int _currentHealth;
    public float cooldownTime;
    private float _cooldownStart;
    private bool _onCooldown;
    private bool _shieldActive;

    /// <summary>
    /// Current health the shield has.
    /// Read only.
    /// </summary>
    public int health
    {
        get => _currentHealth; 
    }

    /// <summary>
    /// Returns true if Shield cooldown is active and cannot be used.
    /// Read only.
    /// </summary>
    public bool cooldownActive
    {
        get => _onCooldown;
    }

    /// <summary>
    /// Returns true if Shield is active at the moment.
    /// Read only.
    /// </summary>
    public bool isActive
    {
        get => _shieldActive;
    }

    [Header("Visual Components")]
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
        // Cooldown timer end check
        if (Time.time - _cooldownStart > cooldownTime)
        {
            _currentHealth = maxHealth;
            _onCooldown = false;
        }
    }

    /// <summary>
    /// Removes one health point from the shield and checks for low health
    /// </summary>
    public void DoDamage()
    {
        _currentHealth--;
        
        if (_currentHealth <= 0)
        {
            _cooldownStart = Time.time;
            _onCooldown = true;
            _shieldActive = false;
        }
    }

    public void OnShield(InputAction.CallbackContext context)
    {
        if (context.action.name != "Shield")
            return;
        
        if (_onCooldown)
            return;
        
        if (context.performed)
        {
            _shieldActive = true;
            
            shieldRenderer.enabled = true;
            shieldCollider.enabled = true;
        }

        if (context.canceled)
        {
            _shieldActive = false;
            
            shieldRenderer.enabled = false;
            shieldCollider.enabled = false;
        }
    }
}
