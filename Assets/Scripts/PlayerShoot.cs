using UnityEngine.InputSystem;
using System;
using Unity.Collections;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    //exposed variables
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    [Range(0.1f, 1.0f)] public float fireRate;
    
    //Internal Variables
    private float _nextFire = 0.0f;
    private float _wantsToFire = 0.0f;
    private PlayerStats _stats;

    void Start()
    {
        _stats = GetComponent<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        //this is outside _wantsToFire if statement to prevent spamming fire button
        // Have to do fireRate/stat because fireRate is not rate is time !!! f=1/l !!! -x
        if (_nextFire < fireRate / _stats.GetStat(StatID.fireRateMultiplier))
        {
            //Add time
            _nextFire += Time.deltaTime;
        }

        //main check
        if (_wantsToFire > 0.5f && (_nextFire >= fireRate / _stats.GetStat(StatID.fireRateMultiplier)))
        {
            Fire();
            //revert timer to 0
            _nextFire = 0.0f;
        }
    }

    private void OnFire(InputValue value)
    {
        //fuck unity, a fucking button does not return bool it returns a fucking float, thanks unity -d
        //xd why aren't we in godot? -x
        _wantsToFire = value.Get<float>();
    }

    private void Fire()
    {
        // Debug.Log("Fire");
        //Instantiate bullet in given spawn location
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
        bullet.GetComponent<BulletScript>().speed *= _stats.GetStat(StatID.bulletSpeedMultiplier);


    }
}
