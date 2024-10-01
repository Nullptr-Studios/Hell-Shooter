using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripleSettings : MonoBehaviour
{
    public List<DefaultWeapon> weaponEmitters;
    
    [Header("Settings")]
    [Range(20f, 60f)] public float coneAngle = 45f;
    [Range(0.1f, 1.0f)] public float fireRate = 0.4f;
    
    void Start()
    {
        weaponEmitters[0].transform.eulerAngles = new Vector3(0f, 0f, 90 - (coneAngle/2));
        weaponEmitters[1].transform.eulerAngles = new Vector3(0f, 0f, 90);
        weaponEmitters[2].transform.eulerAngles = new Vector3(0f, 0f, 90 + (coneAngle/2));
        
        foreach (var weapon in weaponEmitters)
        {
            weapon.fireRate = fireRate;
        }
    }
}
