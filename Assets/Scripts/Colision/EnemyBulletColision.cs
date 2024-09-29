using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyBulletColision : MonoBehaviour
{
    public GameObject player;

    public float radius = 1.0f;
    public PlayerBulletColision playerCollider;

    private Transform _tr;
    private Transform _pTr;
    private Vector2 _pos;
    private Vector2 _playerPos;

#if UNITY_EDITOR
    [Header("Debug")]
    [SerializeField] private bool drawGizmos = true;

    private void OnDrawGizmos()
    {
        if (!drawGizmos)
            return;
        
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
#endif

    private void Start()
    {
        if (player)
        {
            _tr = this.transform;
            _pTr = player.transform;
        }
        else
        {
            Destroy(this);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (player)
        {
            _pos = _tr.position;
            _playerPos = _pTr.position;
            if (TouchesPlayer())
            {
                this.SendMessage("MyTriggerEnter");
                Destroy(this);
            }
        }
    }
    
    private bool TouchesPlayer()
    {
        //returnVector2.Distance(unit.Position(), Position())- unit.Radius()- Radius()<= 0f;
        return ((_pos.x - _playerPos.x) * (_pos.x - _playerPos.x) + (_pos.y - _playerPos.y) * (_pos.y - _playerPos.y)) -
            (playerCollider.playerRadius + radius) <= 0f;
    }
}
