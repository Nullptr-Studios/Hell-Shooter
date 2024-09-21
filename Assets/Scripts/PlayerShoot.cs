using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    //exposed variables
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    [Range(0.1f, 1.0f)] public float fireRate;

    //Inputs
    private HellShooter_IA InputActions;
    private InputAction _fire;
    
    //Internal Variables
    private float _nextFire = 0.0f;
    private bool _wantsToFire = false;

    private void Awake()
    {
        InputActions = new HellShooter_IA();
    }

    private void OnEnable()
    {
        //Setup Input Actions
        _fire = InputActions.Player.Fire;
        _fire.Enable();
        
        _fire.performed += OnPressedFire;
        _fire.canceled += OnCanceledFire;
    }

    private void OnDisable()
    {
        //Disable input actions
        _fire.Disable();
        
        _fire.performed -= OnPressedFire;
        _fire.canceled -= OnCanceledFire;
    }

    // Update is called once per frame
    void Update()
    {
        //this is outside _wantsToFire to prevent spamming fire button
        if (_nextFire < fireRate)
        {
            //Add time
            _nextFire += Time.deltaTime;
        }

        //main check
        if (_wantsToFire && (_nextFire >= fireRate))
        {
            Fire();
            //revert timer to 0
            _nextFire = 0.0f;
        }
    }

    private void OnCanceledFire(InputAction.CallbackContext context)
    {
        _wantsToFire = false;
    }

    private void OnPressedFire(InputAction.CallbackContext context)
    {
        _wantsToFire = true;
    }

    private void Fire()
    {
        Debug.Log("Fire");
        //Instantiate bullet in given spawn location
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
        
    }
}
