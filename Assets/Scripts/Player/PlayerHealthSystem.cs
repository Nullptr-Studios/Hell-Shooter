using System;
using System.Collections;
using System.Collections.Generic;
using ToolBox.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class PlayerHealthSystem : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 3; 
    private float currentHealth;
    public int iFrameDuration = 3;
    private float iLastHit;

    public int shieldXp = 100;

    public int current
    {
        get => (int)currentHealth;
    }
    
    [Header("Hit Animation")]
    [SerializeField] private Color flashColor;
    [SerializeField] private float flashBlinkDuration;
    private bool flashOn = false;
    private SpriteRenderer _spriteRenderer;
    private Material _material;
    private float _flashTimer;
    private float _flashBlinkTimer;
    private float _currentFlash;
    
    private GameObject GUI;
    private Dash _dash;
    private Shield _shield;
    
    /// <summary>
    /// This function is true if _dash is null, use it to avoid null references to _dash.
    /// Use this instead of <c>if(_dash != null)</c> as this is far less expensive
    /// </summary>
    private bool _dNullCheck;

    /// <summary>
    /// This function is true if _shield is null, use it to avoid null references to _shield.
    /// Use this instead of <c>if(_shield != null)</c> as this is far less expensive
    /// </summary>
    private bool _sNullCheck;

#if UNITY_EDITOR
    [Header("Debug")]
    [SerializeField] private bool isInvencible = false;
    [SerializeField] private bool logShield = false;
    [SerializeField] private bool logHit = false;
#endif
    
    // Start is called before the first frame update
    void Awake()
    {
        maxHealth = 3 + DataSerializer.Load<float>(SaveDataKeywords.healthLevel);
        currentHealth = maxHealth;
        GUI = GameObject.Find("GUI");
        
        // Hit animation stuff
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _material = _spriteRenderer.material;
        _material.SetColor("_FlashColor", flashColor);
    }

    void Start()
    {
        _dash = GetComponentInChildren<Dash>();
        _dNullCheck = _dash == null;
        
        _shield = GetComponentInChildren<Shield>();
        _sNullCheck = _shield == null;
    }

    /// <summary>
    ///  Custom damage event because unity doesn't have one
    /// </summary>
    /// <param name="damage">amount of damage to deal</param>
    public void DoDamage(float damage)
    {
        // Exit function if debug mode active
#if UNITY_EDITOR
        if (isInvencible) return;
#endif
        
        // Exit function with iFrame
        if (Time.time - iLastHit <= iFrameDuration)
            return;
        
        // Make player invulnerable when dashing
        if (!_dNullCheck && _dash.isDashActive)
            return;
        
        if (!_sNullCheck && _shield.isActive)
        {
            _shield.DoDamage();
            this.SendMessage("GiveScore", shieldXp);
            
#if UNITY_EDITOR
            if (logShield) Debug.Log("Shield used");
#endif
            
        }
        else
        {
            // Subtract health logic
            currentHealth -= damage;
            GUI.SendMessage("DecreaseHealth");

#if UNITY_EDITOR
            if(logHit) Debug.Log("Hit");
#endif

            // Death logic
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                this.SendMessage("SaveData");
                Destroy(this.gameObject);
                SceneManager.LoadScene("MainMenu");
            }
        }
        
        iLastHit = Time.time;
        _currentFlash = 0f;
        flashOn = true;
        _flashTimer = Time.time;
        _flashBlinkTimer = 0;
    }
    
    /// <summary>
    /// Gain Health void
    /// </summary>
    /// <param name="amount">Health to be added to player</param>
    public void GainHealth(float amount)
    {
        currentHealth += MathF.Abs(amount);
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }

    public void Update()
    {
        if (flashOn)
        {
            _flashBlinkTimer += Time.deltaTime/flashBlinkDuration;
            _currentFlash = Mathf.Lerp(0f, 1f, _flashBlinkTimer);
            _material.SetFloat("_FlashAmount", _currentFlash);
            if (_flashBlinkTimer >= flashBlinkDuration)
                _flashBlinkTimer = 0;
            if (Time.time - _flashTimer > iFrameDuration)
            {
                flashOn = false;
                _material.SetFloat("_FlashAmount", 0);
            }
        }
    }
}
