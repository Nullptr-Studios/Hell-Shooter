using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Private variables
    private Rigidbody2D _rb;
    private Vector2 _direction;
    private float _accelTimer = 0;
    private PlayerStats _stats;
    
    // Public variables
    [SerializeField] [Range(1.0f, 30.0f)] private float maxSpeed;
    [SerializeField] [ReadOnly] private float currentSpeed;
    [Header("Acceleration variables")]
    [SerializeField] private float accelerationTime;
    // [SerializeField] private float decelerationTime;
    
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _stats = GetComponent<PlayerStats>();
        
        _stats.DebugPrint(StatID.speedMultiplier);
    }

    // Update is called once per frame
    void Update()
    {
        _rb.velocity += Vector2.Lerp(Vector2.zero, _direction, _accelTimer) * (maxSpeed * Time.deltaTime * _stats.GetStat(StatID.speedMultiplier));
        currentSpeed = _rb.velocity.magnitude;
        if (_accelTimer < 1) _accelTimer += Time.deltaTime / accelerationTime; 
    }

    /**
     *  Grabs direction from Move input.
     *
     *  Called by Player Input component on BroadcastMessage()
     */
    private void OnMove(InputValue value)
    {
        if (value.Get<Vector2>() != Vector2.zero && _direction == Vector2.zero)
            _accelTimer = 0;
        _direction = value.Get<Vector2>();
    }

    private void OnDebug()
    {
        _stats.LevelUp(StatID.criticalHitPercentage);
        _stats.DebugPrint(StatID.criticalHitPercentage);
    }
    
    // TODO: For criticalHitPercentage multiply stat x8
}
