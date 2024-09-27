using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Private variables
    private Rigidbody2D _rb;
    private Vector2 _direction;
    private float _accelTimer;
    private PlayerStats _stats;
    private Dash _dash;

    /// <summary>
    /// This function is true if _dash is null, use it to avoid null references to _dash.
    /// Use this instead of <c>if(_dash != null)</c> as this is far less expensive
    /// </summary>
    private bool _dNullCheck;
    
    public Vector2 dir
    {
        // I am doing this because I don't want _direction to be public
        // It is just good coding practice
        get => _direction;
        set => _direction = value;
    }

    // Public variables
    [Range(10.0f, 300.0f)] public float maxSpeed;
    [Header("Acceleration variables")]
    [Tooltip("Time it takes for the player to accelerate to max speed")]
    [SerializeField] private float accelerationTime;
    
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _stats = GetComponent<PlayerStats>();
        _dash = GetComponentInChildren<Dash>();
        _dNullCheck = _dash == null;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_dNullCheck) _direction *= !_dash.isDashActive? 1f:0f; // Prevents movement to continue if player releases button mid-dash
        // @TODO:: Right now movement stops when dash is finished, fix that 
        _rb.velocity += Vector2.Lerp(Vector2.zero, _direction, _accelTimer) * (maxSpeed * Time.deltaTime * _stats.GetStat(StatID.speedMultiplier));
        if (_accelTimer < 1) _accelTimer += Time.deltaTime / accelerationTime; 
    }

    /**
     *  Grabs direction from Move input.
     *  Called by message broadcast from Player Input component
     *  
     *  <param name="value">Value raw from PlayerInput component</param>>
     */
    private void OnMove(InputValue value)
    {
        // Negates movement if player is dashing
        if (!_dNullCheck && _dash.isDashActive && _dash.disableMovementOnDash) 
            return;
        
        if (value.Get<Vector2>() != Vector2.zero && _direction == Vector2.zero)
            _accelTimer = 0;
        _direction = value.Get<Vector2>();
    }

    // Calls InputAction OnDebug for testing purposes
    private void OnDebug()
    {
        this.SendMessage("SaveData");
    }
}
