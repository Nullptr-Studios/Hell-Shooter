using System;
using UnityEngine;

public class PlayerBulletColision : MonoBehaviour
{
    public float defaultRadius = 0.05f;
    [NonSerialized] public float playerRadius = 0.05f;

    void Start()
    {
        playerRadius = defaultRadius;
    }

#if UNITY_EDITOR
    [Header("Debug")]
    [SerializeField] private bool drawGizmos = true;

    private void OnDrawGizmos()
    {
        if (!drawGizmos)
            return;
        
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, playerRadius);
    }
#endif
    
}
