using System;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D m_Rigidbody;
    private Vector2 m_Direction;
    [NonSerialized] public bool IsInput = false, IsMoving = false;
    [SerializeField] [ReadOnly] private float m_CurrentSpeed = 0f;
    [SerializeField] MotionController m_MovementController;
    
    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        IsInput = m_Direction != Vector2.zero;
        IsMoving = m_Rigidbody.velocity != Vector2.zero;
        m_MovementController.Update(IsInput, IsMoving);
        m_Rigidbody.velocity = m_Direction * m_MovementController.speed;
        m_CurrentSpeed = m_Rigidbody.velocity.magnitude;
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
