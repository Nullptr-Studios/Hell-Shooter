using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D m_Rigidbody;
    private Vector2 m_Direction;
    [NonSerialized] public bool IsMoving = false;
    public float maxSpeed = 4f;
    [SerializeField] private float m_CurrentSpeed = 0f;
    
    // Acceleration variables
    public AnimationCurve accelerationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    public AnimationCurve decelerationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    public float accelerationTime = 0.5f;
    public float decelerationTime = 0.5f;
    
    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        IsMoving = m_Rigidbody.velocity != Vector2.zero;
        m_Rigidbody.velocity = m_Direction * maxSpeed;
    }

    /**
     *  Grabs direction from Move input.
     *
     *  Called by Player Input component on BroadcastMessage()
     */
    private void OnMove(InputValue value)
    {
        m_Direction = value.Get<Vector2>();
    }
}
