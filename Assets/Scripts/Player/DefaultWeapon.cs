using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.Serialization;

public class DefaultWeapon : MonoBehaviour
{
    //exposed variables
    public GameObject bulletPrefab;
    [Range(0.1f, 1.0f)] public float fireRate;
    
#if UNITY_EDITOR
    [Header("Debug")]
    [SerializeField] private bool logFire = false;
#endif
    
    //Internal Variables
    private float _nextFire;
    private float _wantsToFire;
    private GameObject _player;
    private PlayerStats _stats;
    private PlayerInput _playerInput;

    void Start()
    {
        _player = GameObject.FindWithTag("Player");
        _stats = _player.GetComponent<PlayerStats>();
        if (_stats == null) Debug.LogWarning("No player stats attached to player");
        
        _playerInput = _player.GetComponent<PlayerInput>();
        _playerInput.onActionTriggered += OnFire;
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

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.action.name != "Fire")
            return;
        
        //fuck unity, a fucking button does not return bool it returns a fucking float, thanks unity -d
        //xd why aren't we in godot? -x
        _wantsToFire = context.ReadValue<float>();
    }

    private void Fire()
    {
        
#if UNITY_EDITOR
        if (logFire) Debug.Log("Fire");
#endif

        //Instantiate bullet in given spawn location
        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        bullet.GetComponent<BulletScript>().speed *= _stats.GetStat(StatID.bulletSpeedMultiplier);
    }
}
