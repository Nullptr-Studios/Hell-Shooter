using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    // Private variables
    private Rigidbody2D _rb;
    private Vector2 _direction;
    private float _accelTimer;
    private PlayerStats _stats;
    private Dash _dash;

    private PlayerInput _playerInput;

    /// <summary>
    /// This function is true if _dash is null, use it to avoid null references to _dash.
    /// Use this instead of <c>if(_dash != null)</c> as this is far less expensive
    /// </summary>
    private bool _dNullCheck;
    
    /// <summary>
    /// Raw value of the movement input. Read only.
    /// </summary>
    public Vector2 dir
    {
        // I am doing this because I don't want _direction to be public
        // It is just good coding practice
        get => _direction;
    }

    public GameObject PlayerThrusterRenderer;

    // Public variables
    [Range(10.0f, 300.0f)] public float maxSpeed;
    [Header("Acceleration variables")]
    [Tooltip("Time it takes for the player to accelerate to max speed")]
    [SerializeField] private float accelerationTime;

#if UNITY_EDITOR
    [FormerlySerializedAs("debug")]
    [Header("Debug")]
    [SerializeField] private bool log;
#endif

    // Start is called before the first frame update
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _stats = GetComponent<PlayerStats>();
        
        _playerInput = GetComponent<PlayerInput>();
        _playerInput.onActionTriggered += OnDebug;
        _playerInput.onActionTriggered += OnMove;
        
        _dash = GetComponentInChildren<Dash>();
        _dNullCheck = _dash == null;
        
        PlayerThrusterRenderer.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        if (!_dNullCheck && _dash.isDashActive)
        {
            _rb.velocity += Vector2.Lerp(Vector2.zero, _dash.direction, _accelTimer) * (_dash.speed * Time.deltaTime * _stats.GetStat(StatID.speedMultiplier));
        }
        else
        {
            _rb.velocity += Vector2.Lerp(Vector2.zero, _direction, _accelTimer) * (maxSpeed * Time.deltaTime * _stats.GetStat(StatID.speedMultiplier));
        }

        //Thruster renderer logic
        if (_rb.velocity.magnitude < 3)
        {
            PlayerThrusterRenderer.SetActive(false);
        }
        else
        {
            PlayerThrusterRenderer.SetActive(true);
        }
        
        if (_accelTimer < 1) _accelTimer += Time.deltaTime / accelerationTime; 
    }

    /// <summary>
    /// Grabs direction from Move input.
    /// Called by message broadcast from Player Input component
    /// </summary>
    /// <param name="value">Value raw from PlayerInput component</param>
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.action.name != "Move")
            return;
        
        // // Negates movement if player is dashing
        // if (!_dNullCheck && _dash.isDashActive && _dash.disableMovementOnDash) 
        //     return;

        if (context.ReadValue<Vector2>() != Vector2.zero && _direction == Vector2.zero)
            _accelTimer = 0;
        _direction = context.ReadValue<Vector2>();
    }

    // Calls InputAction OnDebug for testing purposes
    public void OnDebug(InputAction.CallbackContext context)
    {
        
        if (context.action.name != "Debug")
            return;
        
#if UNITY_EDITOR
        if (!context.performed) 
            return;
        if (log) SendMessage("SaveData");
#endif
        
    }
}
