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

    /// <summary>
    /// Returns a value from 0 to 1 that indicates how much time is left until the ability is not on cooldown
    /// </summary>
    public float cProgress;

    [Header("Audio")]
    public AudioSource audioSource;

    public AudioClip shieldUpSound;
    public AudioClip shieldDownSound;
    public AudioClip shieldDeniedSound;
    public AudioClip cooldownFinalizedSound;
    public AudioClip shieldDestroyedSound;

    
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
        if (Time.time < cooldownTime && _cooldownStart == 0)
            cProgress = 1;
        else
            cProgress = Mathf.Clamp01((Time.time - _cooldownStart)/cooldownTime);
        
        // Cooldown timer end check
        if (Time.time - _cooldownStart > cooldownTime)
        {
            if (_onCooldown)
            {
                _currentHealth = maxHealth;
                _onCooldown = false;

                //Sound
                if (audioSource)
                {
                    audioSource.clip = cooldownFinalizedSound;
                    audioSource.Play();
                }

#if UNITY_EDITOR
                if (logCooldown) Debug.Log("Shield cooldown ended");
#endif
            }
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
            
            //Sound
            if (audioSource)
            {
                audioSource.clip = shieldDestroyedSound;
                audioSource.Play();
            }
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
        {
            //Sound
            if (audioSource && context.performed)
            {
                audioSource.clip = shieldDeniedSound;
                audioSource.Play();
            }
            return;
        }
            
        
        if (context.performed)
        {
            _shieldActive = true;
            _customCollider.playerRadius = 0.7f;
            
            _renderer.enabled = true;
            _collider.enabled = true;
            
            //Sound
            if (audioSource)
            {
                audioSource.clip = shieldUpSound;
                audioSource.Play();
            }
        }

        if (context.canceled)
        {
            _shieldActive = false;
            _customCollider.playerRadius = _customCollider.defaultRadius;
            
            _renderer.enabled = false;
            _collider.enabled = false;
            
            //Sound
            if (audioSource)
            {
                audioSource.clip = shieldDownSound;
                audioSource.Play();
            }
        }
    }
}
