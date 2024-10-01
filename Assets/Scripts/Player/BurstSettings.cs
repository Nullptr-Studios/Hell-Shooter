using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstSettings : MonoBehaviour
{
    public BurstWeapon[] burstWeapons;
    
    [Header("Settings")]
    [Range(0.1f, 2.0f)] public float fireRate;
    public float cooldownMultiplier;
    public float burst;

    void Awake()
    {
        foreach (var weapon in burstWeapons)
        {
            weapon.fireRate = fireRate;
            weapon.cooldownMultiplier = cooldownMultiplier;
            weapon.burst = burst;
        }
    }
}
